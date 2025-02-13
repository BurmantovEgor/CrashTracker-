using CSharpFunctionalExtensions;
using TestApplication.Application.DTO_s;
using TestApplication.DataBase.Entities;

namespace TestApplication.Core.Abstractions.User
{
    public interface IUserService
    {
        public Task<Result<UserDTO>> Register(UserRegisterDTO user);
        public Task<Result<UserDTO>> Login(UserDTOLogin user);
    }
}
