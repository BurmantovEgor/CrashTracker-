using AutoMapper;
using CrashTracker.Core.Abstractions;
using CSharpFunctionalExtensions;
using System.Security.Claims;
using TestApplication.Core.Interfaces.Crash;
using TestApplication.Core.Interfaces.Operations;
using TestApplication.Core.Interfaces.Status;
using TestApplication.Core.Models;
using TestApplication.DataBase.Entities;
using TestApplication.DTO_s;

public class CrashService : ICrashService
{
    private readonly ICrashRepository _repository;
    private readonly IStatusService _statusService;
    private readonly IMapper _mapper;
    private readonly ClaimsPrincipal _userClaims;
    private readonly ICashService _cache;
    private readonly IOperationRepository _operationRepository;

    public CrashService(ICrashRepository repository, IStatusService statusService, IMapper mapper, IHttpContextAccessor contextAccessor, ICashService cache, IOperationRepository operationRepository)
    {
        _repository = repository;
        _statusService = statusService;
        _mapper = mapper;
        _userClaims = contextAccessor.HttpContext.User;
        _cache = cache;
        _operationRepository = operationRepository;
    }

    public async Task<Result<CrashDTO>> Create(CrashDTOCreate dto)
    {
        var crashResult = Crash.Create(dto);
        if (crashResult.IsFailure) return Result.Failure<CrashDTO>(crashResult.Error);
        var userIDStr = _userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid.TryParse(userIDStr, out Guid userId);
        var entity = _mapper.Map<CrashEntity>(dto, opts => opts.Items["UserId"] = userId);
        var saveResult = await _repository.Insert(entity);
        if (saveResult.IsFailure) return Result.Failure<CrashDTO>(saveResult.Error);
        var crashDTO = new CrashDTO
        {
            Id = saveResult.Value.Id,
            Name = saveResult.Value.Name,
            Description = saveResult.Value.Description,
            StatusName = saveResult.Value.CrashStatusId.ToString(),
        };

        return Result.Success(crashDTO);
    }

    public async Task<Result<List<CrashDTO>>> GetAll()
    {
        var result = await _repository.SelectAll();
        if (result.IsFailure) return Result.Failure<List<CrashDTO>>(result.Error);
        var statusIds = result.Value.Select(c => c.CrashStatusId).Distinct().ToList();
        List<StatusEntity> statuses = [];
        var cacheStatusStr = await _cache.GetCacheAsync<List<StatusEntity>>("statusList");
        if (!cacheStatusStr.IsFailure)
        {
            statuses = cacheStatusStr.Value;
            var missingStatusIds = statusIds
            .Where(id => !statuses.Any(s => s.Id == id))
            .ToList();

            var newStatusesTask = _statusService.GetStatusesById(missingStatusIds);
            var updatedStatuses = statuses.ToList();

            if (missingStatusIds.Any())
            {
                var newStatuses = await newStatusesTask;
                updatedStatuses.AddRange(newStatuses);
                await _cache.SetCacheAsync("statusList", updatedStatuses);
            }
        }
        else
        {
            statuses = await _statusService.GetStatusesById(statusIds);
            await _cache.SetCacheAsync<List<StatusEntity>>("statusList", statuses, TimeSpan.FromMinutes(60));
        }

        var crashDTOs = _mapper.Map<List<CrashDTO>>(result.Value);
        foreach (var crashDTO in crashDTOs)
        {
            var status = statuses.FirstOrDefault(s => s.Id.ToString() == crashDTO.StatusName);
            crashDTO.StatusName = status?.Name ?? "Не удалось получить статус";
        }
        return Result.Success(crashDTOs);
    }

    public async Task<Result<CrashDTO>> GetById(Guid id)
    {
        var result = await _repository.SelectById(id);
        if (result.IsFailure) return Result.Failure<CrashDTO>(result.Error);
        var statusName = await _statusService.GetById(result.Value.CrashStatusId);
        var crashDTO = _mapper.Map<CrashDTO>(result.Value);
        crashDTO.StatusName = statusName.Name ?? "Не удалось получить статус";
        return Result.Success(crashDTO);
    }

    public async Task<Result> Remove(Guid id)
    {
        var currCrash = await _repository.SelectById(id);
        if (currCrash.IsFailure) return Result.Failure(currCrash.Error);
        var currUser = _userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!currUser.Equals(currCrash.Value.CreatedById.ToString())) return Result.Failure("Вы не можете удалить данную запись");
        return await _repository.Delete(id);
    }

    public async Task<Result> Update(CrashDTOUpdate dto)
    {
        var result = await _repository.SelectById(dto.Id);
        if (result.IsFailure) return Result.Failure(result.Error);
        var currUser = _userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!currUser.Equals(result.Value.CreatedById.ToString())) return Result.Failure("Вы не можете изменять данную запись");
        result.Value.Name = dto.Name;
        result.Value.Description = dto.Description;
        result.Value.CrashStatusId = dto.Status;
        return await _repository.Update(result.Value);
    }

    public async Task<Result> UpdateProgress(Guid crashId)
    {
        var operationsForCurrentCrash = await _operationRepository.SelectByCrashId(crashId);
        if (operationsForCurrentCrash.IsFailure) return Result.Failure(operationsForCurrentCrash.Error);
        if (operationsForCurrentCrash.Value.Count() == 0) return Result.Success();
        var successOperCount = operationsForCurrentCrash.Value.Where(x => x.IsCompleted == true).Count();
        double progress = 0;
        if (successOperCount >  0)
        {
             progress = (double)successOperCount / operationsForCurrentCrash.Value.Count() * 100;

        }
        var result =  await _repository.UpdateProgress(crashId, progress);
        if (result.IsFailure) return Result.Failure(result.Error);
        return Result.Success();
    }
}
