using Eventer.Application.Interfaces;
using Eventer.Domain.Models;
using FluentValidation;

namespace Eventer.Infrastructure.Validators.Auth
{
    public class UserValidator : AbstractValidator<User>
    {
        private readonly IUniqueFieldChecker _uniqueFieldChecker;

        public UserValidator(IUniqueFieldChecker uniqueFieldChecker)
        {
            _uniqueFieldChecker = uniqueFieldChecker;

            RuleFor(u => u).NotNull().WithMessage("User cannot be null.");

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

            RuleFor(u => u.RefreshToken)
                .NotEmpty().WithMessage("RefreshToken обязателен.")
                .MaximumLength(500).WithMessage("RefreshToken не должен превышать 500 символов.");

            RuleFor(u => u.RefreshTokenExpiryTime)
                .Must(expiry => expiry > DateTime.UtcNow)
                .WithMessage("RefreshToken истёк или недействителен.");
        }
    }
}
