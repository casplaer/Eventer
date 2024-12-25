using Eventer.Application.Interfaces;
using Eventer.Domain.Models;
using FluentValidation;

namespace Eventer.Infrastructure.Validators.Categories
{
    public class EventCategoryValidator : AbstractValidator<EventCategory>
    {

        public EventCategoryValidator(IUniqueFieldChecker uniqueFieldChecker)
        {

            RuleFor(ec => ec.Name)
                .NotEmpty().WithMessage("Название категории обязательно.")
                .MaximumLength(40).WithMessage("Название не может превышать 40 символов.");

            RuleFor(ec => ec.Description)
                .MaximumLength(500).WithMessage("Описание не может превышать 500 символов.");
        }
    }
}
