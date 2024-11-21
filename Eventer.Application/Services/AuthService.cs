using Eventer.Application.Contracts.Auth;
using Eventer.Application.Interfaces.Auth;
using Eventer.Application.Interfaces.Repositories;
using Eventer.Application.Interfaces.Services;
using Eventer.Domain.Models;

namespace Eventer.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> LoginUserAsync(LoginUserRequest request)
        {
            var user = await _unitOfWork.Users.GetByUserNameAsync(request.UserName);

            var result = _passwordHasher.Verify(request.Password, user.PasswordHash);

            if(result == false)
            {
                throw new Exception("Failed to login");
            }

            return "123";
        }

        public async Task RegisterUserAsync(RegisterUserRequest request)
        {
            if(request.Password != request.PasswordConfirm)
            {
                throw new ArgumentException("Пароли не совпадают.");
            }

            var existingUser = await _unitOfWork.Users.GetByUserNameAsync(request.UserName);
            if (existingUser != null)
                throw new ArgumentException("Пользователь с таким именем уже существует.");

            var passwordHash = _passwordHasher.GenerateHash(request.Password);

            var user = User.Create(Guid.NewGuid(), request.UserName, passwordHash, request.Email);

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
