using CrashTracker.Application.DTO_s;
using CSharpFunctionalExtensions;
using TestApplication.Application.DTO_s;
using TestApplication.DataBase.Entities;

namespace TestApplication.Core.Abstractions.User
{
    public interface IUserService
    {
        public Task<Result<AuthResponseDTO>> Register(UserRegisterDTO user);
        public Task<Result<AuthResponseDTO>> Login(UserDTOLogin user);

        public Task<Result<UserDTO>> GetById(Guid id);
        public Task<Result<UserDTO>> GetByEmail(string userEmail);
        public Task<Result<UserDTO>> GetByUserName(string userName);
        public Task<Result<List<UserDTO>>> GetAll();

    }
}
