using Eventer.Application.Interfaces.UseCases.Auth;
using Eventer.Domain.Contracts.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Eventer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IRegisterUserUseCase _registerUserUseCase;
        private readonly ILoginUserUseCase _loginUserUseCase;
        private readonly IRefreshTokenUseCase _refreshTokenUseCase;

        public AuthController(
            IRegisterUserUseCase registerUserUseCase,
            ILoginUserUseCase loginUserUseCase,
            IRefreshTokenUseCase refreshTokenUseCase)
        {
            _registerUserUseCase = registerUserUseCase;
            _loginUserUseCase = loginUserUseCase;
            _refreshTokenUseCase = refreshTokenUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
        {
            await _registerUserUseCase.Execute(request, cancellationToken);
            return Ok(new { Message = "Пользователь успешно зарегистрирован." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _loginUserUseCase.Execute(request, cancellationToken);
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody]RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var token = await _refreshTokenUseCase.Execute(request.RefreshToken, cancellationToken);
            return Ok(token);
        }
    }
}
