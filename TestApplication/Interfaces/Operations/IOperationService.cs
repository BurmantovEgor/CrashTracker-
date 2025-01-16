using CSharpFunctionalExtensions;
using TestApplication.DTO_s;

namespace TestApplication.Interfaces.Operations
{
    public interface IOperationService
    {
        Task<Result> Create(Guid crashId, string description);
        Task<Result> Update(OperationDTO operationDTO);
        Task<Result> Remove(Guid id);
        Task<Result<List<OperationDTO>>> GetByCrashId(Guid crashId);
    }
}
