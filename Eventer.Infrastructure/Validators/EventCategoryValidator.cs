using Eventer.Application.Interfaces;
using Eventer.Domain.Models;
using FluentValidation;

namespace Eventer.Infrastructure.Validators
{
    public class EventCategoryValidator : AbstractValidator<EventCategory>
    {
        private readonly IUniqueFieldChecker _uniqueFieldChecker;

        public EventCategoryValidator(IUniqueFieldChecker uniqueFieldChecker)
        {
            _uniqueFieldChecker = uniqueFieldChecker;

            RuleFor(ec => ec.Name)
                .NotEmpty().WithMessage("Название категории обязательно.")
                .MaximumLength(40).WithMessage("Название не может превышать 40 символов.")
                .MustAsync(async (name, cancellation) =>
                    await _uniqueFieldChecker.IsUniqueAsync<EventCategory>("Name", name))
                .WithMessage("Категория с таким названием уже существует."); ;

            RuleFor(ec => ec.Description)
                .MaximumLength(500).WithMessage("Описание не может превышать 500 символов.");
        }
    }
}
