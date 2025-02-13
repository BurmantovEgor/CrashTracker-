using CSharpFunctionalExtensions;
using TestApplication.Application.DTO_s;
using TestApplication.DataBase.Entities;

namespace TestApplication.Core.Abstractions.User
{
    public interface IUserRepository
    {
        public Task<Result<UserDTO>> Register(UserEntity user);
        public Task<Result<UserDTO>> Login(UserDTOLogin user);
    }
}
