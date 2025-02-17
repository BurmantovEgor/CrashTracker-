using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
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
        public async Task<Result<UserEntity>> Login(UserDTOLogin user)
        {
            var userEntity = await _context.User
                .FirstOrDefaultAsync(u => u.UserEmail == user.UserEmail);
            if (userEntity == null) return Result.Failure<UserEntity>("Пользователь с таким Email не найден");
            return Result.Success(userEntity);
        }
        public async Task<Result<AuthResponseDTO>> Register(UserEntity user)
        {
            await _context.User.AddAsync(user);
            var result = await _context.SaveChangesAsync();
            if (result == 0) return Result.Failure<AuthResponseDTO>("Не удалось создать пользователя");
            var userLogin = _mapper.Map<AuthResponseDTO>(user);
            return Result.Success(userLogin);
        }
        public async Task<bool> CheckUserEmail(string userEmail)
        {
            return await _context.User.AnyAsync(u => u.UserEmail == userEmail);
        }
        public async Task<bool> CheckUserName(string userName)
        {
            return await _context.User.AnyAsync(u => u.UserName == userName);
        }

        public async Task<Result<UserEntity>> GetById(Guid id)
        {
            var result = await _context.User
                .FirstOrDefaultAsync(u => u.Id == id);
            if (result == null) return Result.Failure<UserEntity>("Не удалось найти пользователя");
            return Result.Success(result);
        }

        public async Task<Result<UserEntity>> GetByEmail(string userEmail)
        {
            var result = await _context.User
                .FirstOrDefaultAsync(u => u.UserEmail == userEmail);
            if (result == null) return Result.Failure<UserEntity>("Не удалось найти пользователя");
            return Result.Success(result);
        }

        public async Task<Result<UserEntity>> GetByUserName(string userName)
        {
            var result = await _context.User
                .FirstOrDefaultAsync(u => u.UserName == userName);
            if (result == null) return Result.Failure<UserEntity>("Не удалось найти пользователя");
            return Result.Success(result);
        }

        public async Task<Result<List<UserEntity>>> GetAll()
        {
            var result = await _context.User
                .ToListAsync();
            if (result == null) return Result.Failure<List<UserEntity>>("Ошибка при получении списка пользователей");
            return Result.Success(result);
        }
    }
}
