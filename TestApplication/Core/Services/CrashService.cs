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
    private readonly ICrashRepository _crashRepository;
    private readonly IStatusService _statusService;
    private readonly IMapper _mapper;
    private readonly ClaimsPrincipal _currentUser;
    private readonly ICacheService _cache;
    private readonly IOperationRepository _operationRepository;

    public CrashService(
        ICrashRepository repository,
        IStatusService statusService,
        IMapper mapper,
        IHttpContextAccessor contextAccessor,
        ICacheService cache,
        IOperationRepository operationRepository)
    {
        _crashRepository = repository;
        _statusService = statusService;
        _mapper = mapper;
        _currentUser = contextAccessor.HttpContext?.User;
        _cache = cache;
        _operationRepository = operationRepository;
    }

    public async Task<Result<CrashDTO>> Create(CrashDTOCreate dto)
    {
        var creationResult = Crash.Create(dto);
        if (creationResult.IsFailure)
            return Result.Failure<CrashDTO>(creationResult.Error);

        var userIdStr = _currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdStr, out Guid userId))
            return Result.Failure<CrashDTO>("Некорректный идентификатор пользователя");

        var crashEntity = _mapper.Map<CrashEntity>(dto, opts => opts.Items["UserId"] = userId);
        var insertResult = await _crashRepository.Insert(crashEntity);
        if (insertResult.IsFailure)
            return Result.Failure<CrashDTO>(insertResult.Error);

        var getResult = await _crashRepository.GetById(crashEntity.Id);
        if (getResult.IsFailure)
            return Result.Failure<CrashDTO>(getResult.Error);

        var statusName = await GetStatusName(getResult.Value.CrashStatusId);
        var crashDto = _mapper.Map<CrashDTO>(getResult.Value);
        crashDto.StatusName = statusName;

        return Result.Success(crashDto);
    }

    public async Task<Result<List<CrashDTO>>> GetAll()
    {
        var getAllResult = await _crashRepository.GetAll();
        if (getAllResult.IsFailure)
            return Result.Failure<List<CrashDTO>>(getAllResult.Error);

        var crashEntities = getAllResult.Value;
        var statusIds = crashEntities.Select(c => c.CrashStatusId).Distinct().ToList();
        var statusDict = await GetStatusesDictionary(statusIds);

        var crashDTOs = _mapper.Map<List<CrashDTO>>(crashEntities);
        foreach (var crashDto in crashDTOs)
        {
            if (Guid.TryParse(crashDto.StatusName, out Guid statusId) &&
                statusDict.TryGetValue(statusId, out var statusEntity))
            {
                crashDto.StatusName = statusEntity.Name;
            }
            else
            {
                crashDto.StatusName = "Не удалось получить статус";
            }
        }
        return Result.Success(crashDTOs);
    }

    public async Task<Result<CrashDTO>> GetById(Guid id)
    {
        var getResult = await _crashRepository.GetById(id);
        if (getResult.IsFailure)
            return Result.Failure<CrashDTO>(getResult.Error);

        var crashEntity = getResult.Value;
        var statusEntity = await _statusService.GetById(crashEntity.CrashStatusId);
        var crashDto = _mapper.Map<CrashDTO>(crashEntity);
        crashDto.StatusName = statusEntity.Name ?? "Не удалось получить статус";
        return Result.Success(crashDto);
    }

    public async Task<Result<List<CrashDTO>>> GetByUserId(Guid userId)
    {
        var getResult = await _crashRepository.GetByUserId(userId);
        if (getResult.IsFailure)
            return Result.Failure<List<CrashDTO>>(getResult.Error);

        var crashEntities = getResult.Value;
        var statusIds = crashEntities.Select(c => c.CrashStatusId).Distinct().ToList();
        var statusDict = await GetStatusesDictionary(statusIds);

        var crashDTOs = _mapper.Map<List<CrashDTO>>(crashEntities);
        foreach (var crashDto in crashDTOs)
        {
            if (Guid.TryParse(crashDto.StatusName, out Guid statusId) &&
                statusDict.TryGetValue(statusId, out var statusEntity))
            {
                crashDto.StatusName = statusEntity.Name;
            }
            else
            {
                crashDto.StatusName = "Не удалось получить статус";
            }
        }
        return Result.Success(crashDTOs);
    }

    public async Task<Result> Remove(Guid id)
    {
        var crashResult = await _crashRepository.GetById(id);
        if (crashResult.IsFailure)
            return Result.Failure(crashResult.Error);

        var currentUserId = _currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId != crashResult.Value.CreatedById.ToString())
            return Result.Failure("Вы не можете удалить данную запись");

        return await _crashRepository.Delete(id);
    }

    public async Task<Result> Update(CrashDTOUpdate dto)
    {
        var getResult = await _crashRepository.GetById(dto.Id);
        if (getResult.IsFailure)
            return Result.Failure(getResult.Error);

        var crashEntity = getResult.Value;
        var currentUserId = _currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId != crashEntity.CreatedById.ToString())
            return Result.Failure("Вы не можете изменять данную запись");

        crashEntity.Name = dto.Name;
        crashEntity.Description = dto.Description;
        crashEntity.CrashStatusId = dto.Status;

        return await _crashRepository.Update(crashEntity);
    }

    public async Task<Result> UpdateProgress(Guid crashId)
    {
        var operationsResult = await _operationRepository.SelectByCrashId(crashId);
        if (operationsResult.IsFailure)
            return Result.Failure(operationsResult.Error);

        var operations = operationsResult.Value;
        if (!operations.Any())
            return Result.Success();

        var completedCount = operations.Count(op => op.IsCompleted);
        double progress = (double)completedCount / operations.Count() * 100;
        var updateResult = await _crashRepository.UpdateProgress(crashId, progress);
        if (updateResult.IsFailure)
            return Result.Failure(updateResult.Error);
        return Result.Success();
    }

    private async Task<string> GetStatusName(Guid statusId)
    {
        const string cacheKey = "statusList";
        var cacheResult = await _cache.GetCacheAsync<List<StatusEntity>>(cacheKey);
        List<StatusEntity> statuses;
        if (!cacheResult.IsFailure)
        {
            statuses = cacheResult.Value;
            if (!statuses.Any(s => s.Id == statusId))
            {
                var status = await _statusService.GetById(statusId);
                statuses.Add(status);
                await _cache.SetCacheAsync(cacheKey, statuses);
                return status.Name;
            }
        }
        else
        {
            var status = await _statusService.GetById(statusId);
            statuses = new List<StatusEntity> { status };
            await _cache.SetCacheAsync(cacheKey, statuses, TimeSpan.FromMinutes(60));
            return status.Name;
        }
        var existingStatus = statuses.FirstOrDefault(s => s.Id == statusId);
        return existingStatus?.Name ?? "Не удалось получить статус";
    }

    private async Task<Dictionary<Guid, StatusEntity>> GetStatusesDictionary(List<Guid> statusIds)
    {
        const string cacheKey = "statusList";
        var cacheResult = await _cache.GetCacheAsync<List<StatusEntity>>(cacheKey);
        List<StatusEntity> statuses;
        if (!cacheResult.IsFailure)
        {
            statuses = cacheResult.Value;
            var missingIds = statusIds.Where(id => !statuses.Any(s => s.Id == id)).ToList();
            if (missingIds.Any())
            {
                var missingStatuses = await _statusService.GetStatusesById(missingIds);
                statuses.AddRange(missingStatuses);
                await _cache.SetCacheAsync(cacheKey, statuses);
            }
        }
        else
        {
            statuses = await _statusService.GetStatusesById(statusIds);
            await _cache.SetCacheAsync(cacheKey, statuses, TimeSpan.FromMinutes(60));
        }
        return statuses.ToDictionary(s => s.Id, s => s);
    }

}
