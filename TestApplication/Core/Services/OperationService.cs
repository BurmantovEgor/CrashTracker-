using CrashTracker.Core;
using CrashTracker.Core.Helpers;
using CSharpFunctionalExtensions;
using MediatR;
using TestApplication.Core.Interfaces.Operations;
using TestApplication.Core.Models;
using TestApplication.DataBase.Entities;
using TestApplication.DTO_s;

public class OperationService : IOperationService
{
    private readonly IOperationRepository _operationRepository;
    private readonly IMediator _mediator;
    public OperationService(IOperationRepository operationRepository, IMediator mediator)
    {
        _operationRepository = operationRepository;
        _mediator = mediator;
    }

    public async Task<Result> Create(Guid crashId, string description)
    {
        var operationResult = Operation.Create(crashId, description);
        if (operationResult.IsFailure) return Result.Failure(operationResult.Error);

        var entity = new OperationEntity
        {
            Id = operationResult.Value.Id,
            CrashId = operationResult.Value.CrashId,
            Description = operationResult.Value.Description,
            IsCompleted = false
        };
        var result = await _operationRepository.Insert(entity);

        if (result.IsFailure) return Result.Failure(result.Error);
        await _mediator.Publish(new OperationChangedEvent(crashId));
        return Result.Success();
    }

    public async Task<Result<List<OperationDTO>>> GetByCrashId(Guid crashId)
    {
        var result = await _operationRepository.SelectByCrashId(crashId);
        if (result.IsFailure) return Result.Failure<List<OperationDTO>>(result.Error);
        var operationDTOs = result.Value.Select(entity => new OperationDTO(entity.Id, entity.Description, entity.IsCompleted)).ToList();
        return Result.Success(operationDTOs);
    }

    public async Task<Result> Update(OperationDTO operationDTO)
    {
        var resultOperation = await _operationRepository.SelectById(operationDTO.Id);
        if (resultOperation.IsFailure) return Result.Failure(resultOperation.Error);
        var updatedOperation = resultOperation.Value;
        updatedOperation.Description = operationDTO.Description;
        updatedOperation.IsCompleted = operationDTO.IsCompleted;
        var result = await _operationRepository.Update(updatedOperation);
        if (result.IsFailure) return Result.Failure(result.Error);
        await _mediator.Publish(new OperationChangedEvent(updatedOperation.CrashId));
        return Result.Success();
    }

    public async Task<Result> Remove(Guid id)
    {
        var operEntity = await _operationRepository.SelectById(id);
        if (operEntity.IsFailure) return Result.Failure(operEntity.Error);
        var result = await _operationRepository.Delete(id);
        if (result.IsFailure) return Result.Failure(result.Error);
        await _mediator.Publish(new OperationChangedEvent(operEntity.Value.CrashId));
        return Result.Success();
    }

}
