using Eventer.Application.Interfaces;
using Eventer.Domain.Models;
using FluentValidation;

namespace Eventer.Infrastructure.Validators.Events
{
    public class EventValidator : AbstractValidator<Event>
    {
        private readonly IUniqueFieldChecker _uniqueFieldChecker;

        public EventValidator(IUniqueFieldChecker uniqueFieldChecker)
        {
            _uniqueFieldChecker = uniqueFieldChecker;

            RuleFor(e => e.Title)
                .NotEmpty().WithMessage("Название необходимо.")
                .MaximumLength(50).WithMessage("Длина названия не должна превышать 50 символов.")
                .MustAsync(async (e, title, cancellation) =>
                    await _uniqueFieldChecker.IsUniqueAsync<Event>("Title", title, e.Id))
                .WithMessage("Событие с таким названием уже существует.");

            RuleFor(e => e.Description)
                .MaximumLength(500).WithMessage("Длина описания не должна превышать 500 символов.");

            RuleFor(e => e.Venue)
                .NotEmpty().WithMessage("Адрес обязателен.")
                .MaximumLength(100).WithMessage("Длина адреса не должна превышать 100 символов.");

            RuleFor(e => e.StartDate)
                .NotEmpty().WithMessage("Дата начала это обязательное поле.");

            RuleFor(e => e.StartTime)
                .NotEmpty().WithMessage("Время начала это обязательное поле.");

            RuleFor(e => e.MaxParticipants)
                .NotNull().WithMessage("Максимальное количество участников это обязательное поле.")
                .GreaterThan(0).WithMessage("Максимальное количество участников должно быть больше нуля.");

        }
    }
}
