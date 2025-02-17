using AutoMapper;
using CrashTracker.Application.DTO_s;
using CrashTracker.Core.Abstractions;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TestApplication.Application.DTO_s;
using TestApplication.Core.Abstractions.User;
using TestApplication.DataBase.Entities;

namespace TestApplication.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly ICacheService _cache;

        public UserService(IUserRepository userRepository, IMapper mapper, IAuthService authService, ICacheService cache)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _authService = authService;   
            _cache = cache;
        }

        public async Task<Result<AuthResponseDTO>> Login(UserDTOLogin user)
        {
            UserEntity currentUser;
            var cachedUser = await _cache.GetCacheAsync<UserEntity>($"user_{user.UserEmail}");
            if (!cachedUser.IsFailure && cachedUser.Value is not null)
            {
                currentUser = cachedUser.Value;
            }
            else
            {
                var result = await _userRepository.Login(user);
                if (result.IsFailure) return Result.Failure<AuthResponseDTO>(result.Error);
                currentUser = result.Value;
                await SaveUserCache(currentUser);
            }
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(user.Password + user.UserEmail, currentUser.PasswordHash);
            if (!isPasswordValid) return Result.Failure<AuthResponseDTO>("Неверный пароль");
            var resultUser = _mapper.Map<AuthResponseDTO>(currentUser);
            resultUser.JWT = _authService.GenerateJwtToken(currentUser);
            return Result.Success(resultUser);
        }

        public async Task<Result<AuthResponseDTO>> Register(UserRegisterDTO user)
        {
            if (await _userRepository.CheckUserEmail(user.UserEmail)) return Result.Failure<AuthResponseDTO>("Пользователь с таким Email уже существует");
            if (await _userRepository.CheckUserName(user.UserName)) return Result.Failure<AuthResponseDTO>("Пользователь с таким UserName уже существует");
            var userEntity = _mapper.Map<UserEntity>(user);
            var result = await _userRepository.Register(userEntity);
            if (result.IsFailure) return Result.Failure<AuthResponseDTO>(result.Error);
            await SaveUserCache(userEntity);
            result.Value.JWT = _authService.GenerateJwtToken(userEntity);
            return Result.Success(result.Value);
        }


        public async Task<Result<UserDTO>> GetByEmail(string userEmail)
        {
            return await GetUserByData(() => _userRepository.GetByEmail(userEmail));
        }

        public async Task<Result<UserDTO>> GetById(Guid id)
        {
            return await GetUserByData(() => _userRepository.GetById(id));
        }

        public async Task<Result<UserDTO>> GetByUserName(string userName)
        {
            return await GetUserByData(() => _userRepository.GetByUserName(userName));
        }

        public async Task<Result<List<UserDTO>>> GetAll()
        {
            var result = await _userRepository.GetAll();
            if(result.IsFailure) return Result.Failure<List<UserDTO>>(result.Error);
            var listUserDto = _mapper.Map<List<UserDTO>>(result.Value);
            return Result.Success(listUserDto);
        }

        private async Task<Result<UserDTO>> GetUserByData(Func<Task<Result<UserEntity>>> getUserFunc)
        {
            var userResult = await getUserFunc();
            if (userResult.IsFailure)
                return Result.Failure<UserDTO>(userResult.Error);

            var userDto = _mapper.Map<UserDTO>(userResult.Value);
            return Result.Success(userDto);
        }

        private async Task SaveUserCache(UserEntity userEntity)
        {
            await _cache.SetCacheAsync($"user_{userEntity.UserEmail}", userEntity, TimeSpan.FromMinutes(15));
        }

    }
}
