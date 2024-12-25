using Eventer.Application.Interfaces.Auth;
using Eventer.Contracts.Requests.Auth;
using Eventer.Domain.Interfaces.Repositories;
using FluentValidation;

namespace Eventer.Infrastructure.Validators.Auth
{
    public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginUserRequestValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(request => request.UserName)
                .NotEmpty().WithMessage("Имя пользователя не может быть пустым.");
        }
    }
}
