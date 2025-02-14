using AutoMapper;
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
        private readonly ICashService _cache;

        public UserService(IUserRepository userRepository, IMapper mapper, IAuthService authService, ICashService cache)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _authService = authService;   
            _cache = cache;
        }

        public async Task<Result<UserDTO>> Login(UserDTOLogin user)
        {
            UserEntity currentUser;
            
            var cachedUser = await _cache.GetCacheAsync<UserEntity>($"user_{user.UserEmail}");
       
            if (!cachedUser.IsFailure && !(cachedUser.Value == null))
            {
                currentUser = cachedUser.Value;
            }
            else
            {
                var result = await _userRepository.Login(user);
                if (result.IsFailure) return Result.Failure<UserDTO>(result.Error);
                currentUser = result.Value;
                await _cache.SetCacheAsync($"user_{currentUser.UserEmail}", currentUser, TimeSpan.FromMinutes(5));
            }


            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(user.Password + user.UserEmail, currentUser.PasswordHash);
            if (!isPasswordValid) return Result.Failure<UserDTO>("Неверный пароль");
            var resultUser = _mapper.Map<UserDTO>(currentUser);
            resultUser.JWT = _authService.GenerateJwtToken(currentUser);
            return Result.Success(resultUser);
        }

        public async Task<Result<UserDTO>> Register(UserRegisterDTO user)
        {
            if (await _userRepository.CheckUserEmail(user.UserEmail)) return Result.Failure<UserDTO>("Пользователь с таким Email уже существует");
            if (await _userRepository.CheckUserName(user.UserName)) return Result.Failure<UserDTO>("Пользователь с таким UserName уже существует");

            var userEntity = _mapper.Map<UserEntity>(user);
            var result = await _userRepository.Register(userEntity);
            if (result.IsFailure) return Result.Failure<UserDTO>(result.Error);
            result.Value.JWT = _authService.GenerateJwtToken(userEntity);
            return Result.Success(result.Value);
        }
    }
}
