using Eventer.Application.Contracts.Auth;
using Eventer.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Eventer.API.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterUserRequest request)
        {
            await _authService.RegisterUserAsync(request);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginUserRequest request)
        {
            var token = await _authService.LoginUserAsync(request);

            return Ok(token);
        }
    }
}
