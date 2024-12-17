using Eventer.Application.Interfaces;
using Eventer.Domain.Models;
using FluentValidation;

namespace Eventer.Infrastructure.Validators
{
    public class EventRegistrationValidator : AbstractValidator<EventRegistration>
    {
        private readonly IUniqueFieldChecker _uniqueFieldChecker;

        public EventRegistrationValidator(IUniqueFieldChecker uniqueFieldChecker)
        {
            _uniqueFieldChecker = uniqueFieldChecker;

            RuleFor(er => er.Name)
                .NotEmpty().WithMessage("Имя обязательно.")
                .MaximumLength(100).WithMessage("Имя не может превышать 100 символов.");

            RuleFor(er => er.Surname)
                .NotEmpty().WithMessage("Фамилия обязательна.")
                .MaximumLength(100).WithMessage("Фамилия не может превышать 100 символов.");

            RuleFor(er => er.Email)
                .NotEmpty().WithMessage("Поле Email обязательно.")
                .MaximumLength(255).WithMessage("Поле Email не может превышать 255 символов.")
                .EmailAddress().WithMessage("Некорректный формат Email.")
                .MustAsync(async (email, cancellation) =>
                    await _uniqueFieldChecker.IsUniqueAsync<EventRegistration>("Email", email))
                .WithMessage("Пользователь с таким Email уже зарегистрирован."); ;

            RuleFor(er => er.DateOfBirth)
                .NotEmpty().WithMessage("Дата рождения обязательна.");
        }
    }
}
