using Eventer.Domain.Interfaces.Repositories;
using FluentValidation;

namespace Eventer.Infrastructure.Validators.Auth
{
    public class RefreshTokenValidator : AbstractValidator<string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(token => token)
                .NotEmpty().WithMessage("Refresh token cannot be empty.")
                .MustAsync(async (token, cancellation) =>
                {
                    var user = await _unitOfWork.Users.GetByRefreshTokenAsync(token, cancellation);
                    return user != null && user.RefreshTokenExpiryTime > DateTime.UtcNow;
                }).WithMessage("Неправильный или истекший refresh token.");
        }
    }
}
