using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoboAdvisorApp.API.Models.DTO;
using RoboAdvisorApp.API.Models.DTO.Request;
using RoboAdvisorApp.API.Services.Interface;

namespace RoboAdvisorApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            var user = await _userService.RegisterAsync(userDto);
            return Ok(user);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequest request)
        {
            var user = await _userService.AuthenticateAsync(request.Username, request.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
    }
}
