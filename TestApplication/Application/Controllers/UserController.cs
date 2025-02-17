using CrashTracker.Application.DTO_s;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TestApplication.Application.DTO_s;
using TestApplication.Core.Abstractions.User;

namespace TestApplication.Application.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Авторизация и аутентификация.
        /// </summary>
        [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDTOLogin user)
        {
            var result = await _userService.Login(user);
            if (result.IsFailure) return NotFound(result.Error);
            return Ok(result.Value);
        }
        /// <summary>
        /// Регистрация пользователя.
        /// </summary>
        [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO userRegister)
        {
            var result = await _userService.Register(userRegister);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }
        /// <summary>
        /// Получить пользователя по Email.
        /// </summary>
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet("by-email/{email}")]
        public async Task<IActionResult> Email(string email)
        {
            var result = await _userService.GetByEmail(email);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }
        /// <summary>
        /// Получить пользователя по UserName.
        /// </summary>
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet("by-username/{userName}")]
        public async Task<IActionResult> UserName(string userName)
        {
            var result = await _userService.GetByUserName(userName);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }
        /// <summary>
        /// Получить пользователя по ID.
        /// </summary>
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet("by-id/{id}")]
        public async Task<IActionResult> Id(Guid id)
        {
            var result = await _userService.GetById(id);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        /// <summary>
        /// Получить список всех пользователей.
        /// </summary>
        [ProducesResponseType(typeof(List<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet("all")]
        public async Task<IActionResult> All()
        {
            var result = await _userService.GetAll();
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

    }
}
