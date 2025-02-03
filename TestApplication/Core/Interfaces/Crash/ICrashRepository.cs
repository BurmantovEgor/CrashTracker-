using CSharpFunctionalExtensions;
using TestApplication.DataBase.Entities;
using TestApplication.DTO_s;

namespace TestApplication.Core.Interfaces.Crash
{
    public interface ICrashRepository
    {



        Task<Result<CrashEntity>> Insert(CrashEntity entity);
        Task<Result<List<CrashEntity>>> SelectAll();
        Task<Result<CrashEntity>> SelectById(Guid id);
        Task<Result> Delete(Guid id);
        Task<Result> Update(CrashEntity entity);
    }
}
