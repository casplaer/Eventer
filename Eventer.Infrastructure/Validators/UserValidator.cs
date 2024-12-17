using Eventer.Application.Interfaces;
using Eventer.Domain.Models;
using FluentValidation;

namespace Eventer.Infrastructure.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        private readonly IUniqueFieldChecker _uniqueFieldChecker;

        public UserValidator(IUniqueFieldChecker uniqueFieldChecker)
        {
            _uniqueFieldChecker = uniqueFieldChecker;

            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("Это поле обязательное.")
                .MaximumLength(50).WithMessage("Имя пользователя не должно превышать 50 символов.")
                .MustAsync(async (username, cancellation) =>
                    await _uniqueFieldChecker.IsUniqueAsync<User>("UserName", username))
                .WithMessage("Это имя пользователя уже занято.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Поле Email обязательное.")
                .MaximumLength(255).WithMessage("Email не должен превышать 255 символов.")
                .EmailAddress().WithMessage("Некорректный формат Email.");
        }
    }
}
