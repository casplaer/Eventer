using Eventer.Application.Interfaces.UseCases.Auth;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;
using FluentValidation;

namespace Eventer.Application.UseCases.Auth
{
    public class GetUserByTokenUseCase : IGetUserByTokenUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByTokenUseCase(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Execute(string refreshToken, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByRefreshTokenAsync(refreshToken, cancellationToken);

            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Пожалуйста, авторизуйтесь ещё раз.");
            }

            return user;
        }
    }

}
