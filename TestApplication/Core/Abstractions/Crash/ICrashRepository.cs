using CSharpFunctionalExtensions;
using TestApplication.DataBase.Entities;
using TestApplication.DTO_s;

namespace TestApplication.Core.Interfaces.Crash
{
    public interface ICrashRepository
    {
        Task<Result<Guid>> Insert(CrashEntity entity);
        Task<Result<List<CrashEntity>>> GetAll();
        Task<Result<CrashEntity>> GetById(Guid id);
        Task<Result<List<CrashEntity>>> GetByUserId(Guid id);
        Task<Result> Delete(Guid id);
        Task<Result> Update(CrashEntity entity);
        Task<Result> UpdateProgress(Guid crashId, double progress);

    }
}
