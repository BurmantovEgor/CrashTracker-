using CSharpFunctionalExtensions;
using TestApplication.Application.DTO_s;
using TestApplication.DataBase.Entities;

namespace TestApplication.Core.Abstractions.User
{
    public interface IUserRepository
    {
        public Task<Result<UserDTO>> Register(UserEntity user);
        public Task<Result<UserEntity>> Login(UserDTOLogin user);

        public Task<bool> CheckUserEmail(string userEmail);
        public Task<bool> CheckUserName(string userEmail);

    }
}
