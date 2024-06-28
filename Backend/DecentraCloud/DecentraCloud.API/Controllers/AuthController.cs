// Controllers/AuthController.cs

using DecentraCloud.API.DTOs;
using DecentraCloud.API.Interfaces;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DecentraCloud.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDto userDto)
        {
            try
            {
                var user = await _userService.RegisterUser(userDto);
                return Ok(user);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userDto)
        {
            try
            {
                var token = await _userService.LoginUser(userDto);
                return Ok(new { Token = token });
            }
            catch (System.Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
