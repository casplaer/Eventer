using Eventer.Application.Exceptions;
using Eventer.Application.Interfaces.Auth;
using Eventer.Application.Interfaces.UseCases.Auth;
using Eventer.Domain.Interfaces.Repositories;
using FluentValidation;

namespace Eventer.Application.UseCases.Auth
{
    public class RefreshTokenUseCase : IRefreshTokenUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtProvider _jwtProvider;

        public RefreshTokenUseCase(
            IUnitOfWork unitOfWork,
            IJwtProvider jwtProvider)
        {
            _unitOfWork = unitOfWork;
            _jwtProvider = jwtProvider;
        }

        public async Task<string> Execute(string refreshToken, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByRefreshTokenAsync(refreshToken, cancellationToken);

            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                throw new BadRequestException("Некорректный или истекший refresh токен.");
            }

            var newAccessToken = _jwtProvider.GenerateAccessToken(user);

            return newAccessToken;
        }
    }
}
