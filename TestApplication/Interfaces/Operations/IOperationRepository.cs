using CSharpFunctionalExtensions;
using TestApplication.DataBase.Entities;

namespace TestApplication.Interfaces.Operations
{
    public interface IOperationRepository
    {
        Task<Result<OperationEntity>> Insert(OperationEntity operationEntity);
        Task<Result> Update(OperationEntity operationEntity);
        Task<Result> Delete(Guid id);
        Task<Result<List<OperationEntity>>> SelectByCrashId(Guid crashId);
        Task<Result<OperationEntity>> SelectById(Guid crashId);

    }
}
