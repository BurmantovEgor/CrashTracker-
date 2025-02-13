using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TestApplication.Application.DTO_s;
using TestApplication.Core.Abstractions.User;
using TestApplication.DataBase.Configurations;
using TestApplication.DataBase.Entities;

namespace TestApplication.DataBase.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly CrashTrackerDbContext _context;
        private readonly IMapper _mapper;
        public UserRepository(CrashTrackerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<Result<UserDTO>> Login(UserDTOLogin user)
        {
            var userEntity = await _context.User
                .FirstOrDefaultAsync(u => u.UserEmail == user.UserEmail);
            if (userEntity == null) return Result.Failure<UserDTO>("Пользователь не найден");
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(user.Password + userEntity.UserEmail, userEntity.PasswordHash);
            if (!isPasswordValid)return Result.Failure<UserDTO>("Неверный пароль");
            var userDto = _mapper.Map<UserDTO>(userEntity);
            return Result.Success(userDto);
        }




        public async Task<Result<UserDTO>> Register(UserEntity user)
        {
            await _context.User.AddAsync(user);
            var result = await _context.SaveChangesAsync();
            if (result == 0) return Result.Failure<UserDTO>("Не удалось создать пользователя");
            var userLogin = _mapper.Map<UserDTO>(user);
            return Result.Success(userLogin);
        }
    }
}
