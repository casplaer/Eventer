using Eventer.Application.Contracts.Auth;
using Eventer.Application.Interfaces.Auth;
using Eventer.Application.Interfaces.Repositories;
using Eventer.Application.Interfaces.Services;
using Eventer.Domain.Models;

namespace Eventer.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public AuthService(IUnitOfWork unitOfWork, 
            IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<TokensResponse> LoginUserAsync(LoginUserRequest request)
        {
            var user = await _unitOfWork.Users.GetByUserNameAsync(request.UserName);

            if (user == null)
            {
                throw new Exception("No such user.");
            }

            var result = _passwordHasher.Verify(request.Password, user.PasswordHash);

            if(result == false)
            {
                throw new Exception("Failed to login");
            }

            var accessToken = _jwtProvider.GenerateAccessToken(user);
            var refreshToken = _jwtProvider.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new TokensResponse(accessToken, refreshToken);
        }

        public async Task RegisterUserAsync(RegisterUserRequest request)
        {
            if(request.Password != request.PasswordConfirm)
            {
                throw new ArgumentException("Пароли не совпадают.");
            }

            var passwordHash = _passwordHasher.GenerateHash(request.Password);

            var user = User.Create(Guid.NewGuid(), request.UserName, passwordHash, request.Email);

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<TokensResponse> RefreshTokensAsync(string refreshToken)
        {
            var user = await _unitOfWork.Users.GetByRefreshTokenAsync(refreshToken);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }

            var newAccessToken = _jwtProvider.GenerateAccessToken(user);
            var newRefreshToken = _jwtProvider.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _unitOfWork.Users.UpdateAsync(user);

            return new TokensResponse(newAccessToken, newRefreshToken);
        }

        public async Task<User> GetUserByTokenAsync(string refreshToken)
        {
            var user = await _unitOfWork.Users.GetByRefreshTokenAsync(refreshToken);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }

            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var user = await _unitOfWork.Users.GetByUserNameAsync(username);

            if (user == null)
            {
                throw new Exception("No such user.");
            }

            return user;
        }
    }
}
