using CSharpFunctionalExtensions;
using TestApplication.Application.DTO_s;
using TestApplication.DataBase.Entities;

namespace TestApplication.Core.Abstractions.User
{
    public interface IUserRepository
    {
        public Task<Result<AuthResponseDTO>> Register(UserEntity user);
        public Task<Result<UserEntity>> Login(UserDTOLogin user);

        public Task<Result<UserEntity>> GetById(Guid id);
        public Task<Result<UserEntity>> GetByEmail(string userEmail);
        public Task<Result<UserEntity>> GetByUserName(string userName);
        public Task<Result<List<UserEntity>>> GetAll();


        public Task<bool> CheckUserEmail(string userEmail);
        public Task<bool> CheckUserName(string userEmail);

    }
}
