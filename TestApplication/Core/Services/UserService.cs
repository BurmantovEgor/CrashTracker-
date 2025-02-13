using AutoMapper;
using CSharpFunctionalExtensions;
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

        public UserService(IUserRepository userRepository, IMapper mapper, IAuthService authService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _authService = authService;   
        }

        public async Task<Result<UserDTO>> Login(UserDTOLogin user)
        {
            var result = await _userRepository.Login(user);
            if(result.IsFailure) return Result.Failure<UserDTO>(result.Error);
            result.Value.JWT = _authService.GenerateJwtToken(result.Value);
            return Result.Success(result.Value);
        }

        public async Task<Result<UserDTO>> Register(UserRegisterDTO user)
        {
            var userEntity = _mapper.Map<UserEntity>(user);
            var result = await _userRepository.Register(userEntity);
            if (result.IsFailure) return Result.Failure<UserDTO>(result.Error);
            result.Value.JWT = _authService.GenerateJwtToken(result.Value);
            return Result.Success(result.Value);
        }
    }
}
