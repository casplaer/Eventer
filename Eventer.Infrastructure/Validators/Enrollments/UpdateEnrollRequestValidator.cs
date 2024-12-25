﻿using Eventer.Application.Interfaces;
using Eventer.Contracts.Requests.Enrollments;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;
using FluentValidation;

namespace Eventer.Infrastructure.Validators.Enrollments
{
    public class UpdateEnrollRequestValidator : AbstractValidator<UpdateEnrollRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEnrollRequestValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;


            RuleFor(req => req).NotNull().WithMessage("Запрос не может быть пустым.");

            RuleFor(req => req.EnrollmentId)
              .NotEmpty().WithMessage("EnrollmentId не может быть пустым.");

            RuleFor(req => req.Name)
                .NotEmpty().WithMessage("Имя обязательно.")
                .MaximumLength(100).WithMessage("Имя не может превышать 100 символов.");

            RuleFor(req => req.Surname)
                .NotEmpty().WithMessage("Фамилия обязательна.")
                .MaximumLength(100).WithMessage("Фамилия не может превышать 100 символов.");

            RuleFor(req => req.Email)
                .NotEmpty().WithMessage("Поле Email обязательно.")
                .MaximumLength(255).WithMessage("Поле Email не может превышать 255 символов.")
                .EmailAddress().WithMessage("Некорректный формат Email.");

            RuleFor(req => req.DateOfBirth)
                .NotEmpty().WithMessage("Дата рождения обязательна.");
        }
    }
}
