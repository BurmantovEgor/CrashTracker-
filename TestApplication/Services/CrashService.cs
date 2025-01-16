using CSharpFunctionalExtensions;
using TestApplication.DataBase.Entities;
using TestApplication.DTO_s;
using TestApplication.Interfaces.Crash;
using TestApplication.Interfaces.Status;
using TestApplication.Models;

public class CrashService : ICrashService
{
    private readonly ICrashRepository _repository;
    private readonly IStatusService _statusService;

    public CrashService(ICrashRepository repository, IStatusService statusService)
    {
        _repository = repository;
        _statusService = statusService;
    }

    public async Task<Result<CrashDTO>> Create(CrashDTOCreate dto)
    {
        var crashResult = Crash.Create(dto);
        if (crashResult.IsFailure) return Result.Failure<CrashDTO>(crashResult.Error);


        var entity = new CrashEntity
        {
            Id = crashResult.Value.Id,
            Name = crashResult.Value.Name,
            Description = crashResult.Value.Description,
            StatusId = crashResult.Value.StatusId
        };

        var saveResult = await _repository.Insert(entity);
        if (saveResult.IsFailure) return Result.Failure<CrashDTO>(saveResult.Error);

        var crashDTO = new CrashDTO
        {
            Id = saveResult.Value.Id,
            Name = saveResult.Value.Name,
            Description = saveResult.Value.Description,
            StatusName = saveResult.Value.StatusId.ToString(),
        };

        return Result.Success(crashDTO);
    }

    public async Task<Result<List<CrashDTO>>> GetAll()
    {
        var result = await _repository.SelectAll();
        if (result.IsFailure) return Result.Failure<List<CrashDTO>>(result.Error);

        var crashDTOs = new List<CrashDTO>();

        foreach (var entity in result.Value)
        {
            var statusName = await _statusService.GetById(entity.StatusId);
            var crashDTO = new CrashDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                StatusName = statusName.Name ?? "Не удалось получить статус", 
                Operations = entity.Operations
                    .Select(o => new OperationDTO(o.Id, o.Description))
                    .ToList()
            };

            crashDTOs.Add(crashDTO);
        }

        return Result.Success(crashDTOs);
    }

    public async Task<Result<CrashDTO>> GetById(Guid id)
    {
        var result = await _repository.SelectById(id);
        if (result.IsFailure) return Result.Failure<CrashDTO>(result.Error);

        var statusName = await _statusService.GetById(result.Value.StatusId);

        var crashDTO = new CrashDTO
        {
            Id = result.Value.Id,
            Name = result.Value.Name,
            Description = result.Value.Description,
            StatusName = statusName.Name?? "Не удалось получить статус", 
            Operations = result.Value.Operations
                .Select(o => new OperationDTO(o.Id, o.Description))
                .ToList()
        };

        return Result.Success(crashDTO);
    }

    public async Task<Result> Remove(Guid id)
    {
        return await _repository.Delete(id);
    }

    public async Task<Result> Update(CrashDTOUpdate dto)
    {
        
        var result = await _repository.SelectById(dto.Id);
        if (result.IsFailure) return Result.Failure(result.Error);

        result.Value.Name = dto.Name;
        result.Value.Description = dto.Description;
        result.Value.StatusId = dto.Status;

        return await _repository.Update(result.Value);
    }
}
