using Eventer.Application.Interfaces.UseCases.Auth;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;
using FluentValidation;

namespace Eventer.Application.UseCases.Auth
{
    public class GetUserByTokenUseCase : IGetUserByTokenUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<User> _validator;

        public GetUserByTokenUseCase(
            IUnitOfWork unitOfWork,
            IValidator<User> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<User> Execute(string refreshToken, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByRefreshTokenAsync(refreshToken, cancellationToken);

            await _validator.ValidateAndThrowAsync(user, cancellationToken);

            return user;
        }
    }

}
