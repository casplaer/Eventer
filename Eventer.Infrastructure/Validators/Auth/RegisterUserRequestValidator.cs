﻿using Eventer.Domain.Contracts.Auth;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Utilities;
using FluentValidation;

namespace Eventer.Infrastructure.Validators.Auth
{
    public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserRequestValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(r => r.UserName)
                .NotEmpty().WithMessage("Логин обязателен.")
                .MaximumLength(50).WithMessage("Логин не должен превышать 50 символов.")
                .MustAsync(async (username, cancellation) =>
                    await _unitOfWork.Users.GetByUserNameAsync(username, cancellation) == null)
                .WithMessage("Пользователь с таким логином уже существует.");

            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Email обязателен.")
                .MaximumLength(255).WithMessage("Email не должен превышать 255 символов.")
                .EmailAddress().WithMessage("Некорректный формат Email.")
                .MustAsync(async (email, cancellation) =>
                {
                    var normalizedEmail = EmailNormalizer.Normalize(email);
                    return await _unitOfWork.Users.GetByNormalizedEmailAsync(normalizedEmail, cancellation) == null;
                })
                .WithMessage("Этот Email уже используется.");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Пароль обязателен.")
                .MinimumLength(6).WithMessage("Пароль должен содержать минимум 6 символов.");

            RuleFor(r => r.PasswordConfirm)
                .Equal(r => r.Password).WithMessage("Пароли не совпадают.");
        }
    }
}