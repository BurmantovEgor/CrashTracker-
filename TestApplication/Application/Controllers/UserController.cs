using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TestApplication.Application.DTO_s;
using TestApplication.Core.Abstractions.User;

namespace TestApplication.Application.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDTOLogin user)
        {
            var result = await _userService.Login(user);
            if (result.IsFailure) return NotFound(result.Error);
            return Ok(result.Value);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO userRegister)
        {
            var result = await _userService.Register(userRegister);
            if(result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

    }
}
