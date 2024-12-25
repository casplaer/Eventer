using Eventer.Application.Exceptions;
using Eventer.Application.Interfaces.Auth;
using Eventer.Application.Interfaces.UseCases.Auth;
using Eventer.Contracts.Requests.Auth;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;
using Eventer.Domain.Utilities;
using FluentValidation;

namespace Eventer.Application.UseCases.Auth
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserUseCase(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task Execute(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var existingUser = await _unitOfWork.Users.GetByUserNameAsync(request.UserName, cancellationToken);

            if (existingUser != null)
            {
                throw new AlreadyExistsException("Пользователь с таким логином уже существует.");
            }

            var normalizedEmail = EmailNormalizer.Normalize(request.Email);
            existingUser = await _unitOfWork.Users.GetByNormalizedEmailAsync(normalizedEmail, cancellationToken);

            if (existingUser != null)
            {
                throw new AlreadyExistsException("Этот Email уже используется.");
            }

            var passwordHash = _passwordHasher.GenerateHash(request.Password);

            var user = User.Create(Guid.NewGuid(), request.UserName, passwordHash, request.Email);

            await _unitOfWork.Users.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
