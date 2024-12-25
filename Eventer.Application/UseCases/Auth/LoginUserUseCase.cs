using Eventer.Application.Interfaces.Auth;
using Eventer.Application.Interfaces.UseCases.Auth;
using Eventer.Contracts.Requests.Auth;
using Eventer.Contracts.Responses.Auth;
using AutoMapper;
using Eventer.Application.Exceptions;
using Eventer.Contracts.DTOs.Auth;
using Eventer.Domain.Interfaces.Repositories;
using FluentValidation;

namespace Eventer.Application.UseCases.Auth
{
    public class LoginUserUseCase : ILoginUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtProvider _jwtProvider;
        private readonly IPasswordHasher _hasher;
        private readonly IMapper _mapper;

        public LoginUserUseCase(
            IUnitOfWork unitOfWork,
            IJwtProvider jwtProvider,
            IMapper mapper,
            IPasswordHasher hasher)
        {
            _unitOfWork = unitOfWork;
            _jwtProvider = jwtProvider;
            _mapper = mapper;
            _hasher = hasher;
        }

        public async Task<LoginResponse> Execute(LoginUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByUserNameAsync(request.UserName, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException("Такого пользователя не существует.");
            }

            if (!_hasher.Verify(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Некорректные данные для входа.");
            }

            var accessToken = _jwtProvider.GenerateAccessToken(user);
            var refreshToken = _jwtProvider.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            var userDTO = _mapper.Map<UserDTO>(user);

            return new LoginResponse(accessToken, refreshToken, userDTO);
        }
    }

}
