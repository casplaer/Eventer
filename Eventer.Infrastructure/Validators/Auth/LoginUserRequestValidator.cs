using Eventer.Application.Interfaces.Auth;
using Eventer.Domain.Contracts.Auth;
using Eventer.Domain.Interfaces.Repositories;
using FluentValidation;

namespace Eventer.Infrastructure.Validators.Auth
{
    public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public LoginUserRequestValidator(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;

            RuleFor(request => request.UserName)
                .NotEmpty().WithMessage("Имя пользователя не может быть пустым.")
                .MustAsync(async (username, cancellation) =>
                    await _unitOfWork.Users.GetByUserNameAsync(username, cancellation) != null)
                .WithMessage("Такого пользователя не существует.");

            RuleFor(request => request)
                .MustAsync(async (request, cancellation) =>
                {
                    var user = await _unitOfWork.Users.GetByUserNameAsync(request.UserName, cancellation);
                    return user != null && _passwordHasher.Verify(request.Password, user.PasswordHash);
                })
                .WithMessage("Неверный пароль.");
        }
    }
}
