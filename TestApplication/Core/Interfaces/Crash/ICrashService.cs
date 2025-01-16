using CSharpFunctionalExtensions;
using TestApplication.DataBase.Entities;
using TestApplication.DTO_s;

namespace TestApplication.Core.Interfaces.Crash
{
    public interface ICrashService
    {
        Task<Result<CrashDTO>> Create(CrashDTOCreate dto);
        Task<Result<List<CrashDTO>>> GetAll();
        Task<Result<CrashDTO>> GetById(Guid id);
        Task<Result> Remove(Guid id);
        Task<Result> Update(CrashDTOUpdate dto);
    }
}
