using Eventer.Application.Contracts.Auth;
using Eventer.Application.Interfaces.Auth;
using Eventer.Application.Interfaces.UseCases.Auth;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;
using Eventer.Domain.Utilities;
using FluentValidation;

namespace Eventer.Application.UseCases.Auth
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IGetUserByUsernameUseCase _getUserByUsernameUseCase;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IValidator<User> _validator;

        public RegisterUserUseCase(
            IGetUserByUsernameUseCase getUserByUsernameUseCase,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IValidator<User> validator)
        {
            _getUserByUsernameUseCase = getUserByUsernameUseCase;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _validator = validator;
        }

        public async Task Execute(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var normalizedEmail = EmailNormalizer.Normalize(request.Email);

            var existingUser = await _unitOfWork.Users.GetByNormalizedEmailAsync(normalizedEmail, cancellationToken);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Этот Email уже используется.");
            }

            if (request.Password != request.PasswordConfirm)
            {
                throw new ArgumentException("Пароли не совпадают.");
            }

/*            existingUser = await _getUserByUsernameUseCase.Execute(request.UserName, cancellationToken);
            if (existingUser != null)
            {
                throw new ArgumentException("Пользователь с таким логином уже существует.");
            }
*/
            var passwordHash = _passwordHasher.GenerateHash(request.Password);

            var user = User.Create(Guid.NewGuid(), request.UserName, passwordHash, request.Email);

            var validationResult = await _validator.ValidateAsync(user, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _unitOfWork.Users.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
