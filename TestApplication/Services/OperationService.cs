using CSharpFunctionalExtensions;
using TestApplication.DataBase.Entities;
using TestApplication.DTO_s;
using TestApplication.Interfaces.Crash;
using TestApplication.Interfaces.Operations;
using TestApplication.Models;

public class OperationService : IOperationService
{
    private readonly IOperationRepository _operationRepository;
    private readonly ICrashRepository _crashRepository;

    public OperationService(IOperationRepository operationRepository, ICrashRepository crashRepository)
    {
        _operationRepository = operationRepository;
        _crashRepository = crashRepository;
    }

    public async Task<Result> Create(Guid crashId, string description)
    {
        var crashResult = await _crashRepository.SelectById(crashId);
        if (crashResult.IsFailure) return Result.Failure(crashResult.Error);

        var operationResult = Operation.Create(crashId, description);
        if (operationResult.IsFailure) return Result.Failure(operationResult.Error);

        var entity = new OperationEntity
        {
            Id = operationResult.Value.Id,
            CrashId = operationResult.Value.CrashId,
            Description = operationResult.Value.Description
        };

        return await _operationRepository.Insert(entity);
    }

    public async Task<Result<List<OperationDTO>>> GetByCrashId(Guid crashId)
    {
        var result = await _operationRepository.SelectByCrashId(crashId);
        if (result.IsFailure) return Result.Failure<List<OperationDTO>>(result.Error);

        var operationDTOs = result.Value.Select(entity => new OperationDTO(entity.Id, entity.Description)
        ).ToList();

        return Result.Success(operationDTOs);
    }

    public async Task<Result> Update(OperationDTO operationDTO)
    {
        var result = await _operationRepository.SelectById(operationDTO.Id);
        if (result.IsFailure) return Result.Failure(result.Error);

      
        var entity = new OperationEntity
        {
            Id = result.Value.Id,
            CrashId = result.Value.CrashId,
            Description = operationDTO.Description
        };

        return await _operationRepository.Update(entity);
    }

    public async Task<Result> Remove(Guid id)
    {
        return await _operationRepository.Delete(id);
    }
}
