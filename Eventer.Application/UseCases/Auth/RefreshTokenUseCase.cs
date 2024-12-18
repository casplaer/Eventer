using Eventer.Application.Contracts.Auth;
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
        private readonly IValidator<string> _validator;

        public RefreshTokenUseCase(
            IUnitOfWork unitOfWork,
            IJwtProvider jwtProvider,
            IValidator<string> validator)
        {
            _unitOfWork = unitOfWork;
            _jwtProvider = jwtProvider;
            _validator = validator;
        }

        public async Task<string> Execute(string refreshToken, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(refreshToken, cancellationToken);

            var user = await _unitOfWork.Users.GetByRefreshTokenAsync(refreshToken, cancellationToken);

            var newAccessToken = _jwtProvider.GenerateAccessToken(user);

            return newAccessToken;
        }
    }
}
