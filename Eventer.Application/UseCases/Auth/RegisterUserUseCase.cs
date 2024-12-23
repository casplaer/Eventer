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
        private readonly IValidator<RegisterUserRequest> _validator;

        public RegisterUserUseCase(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IValidator<RegisterUserRequest> validator)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _validator = validator;
        }

        public async Task Execute(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var passwordHash = _passwordHasher.GenerateHash(request.Password);

            var user = User.Create(Guid.NewGuid(), request.UserName, passwordHash, request.Email);

            await _unitOfWork.Users.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
