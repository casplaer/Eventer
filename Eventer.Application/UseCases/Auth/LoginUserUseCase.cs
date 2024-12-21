using Eventer.Application.Interfaces.Auth;
using Eventer.Application.Interfaces.UseCases.Auth;
using Eventer.Domain.Interfaces.Repositories;

namespace Eventer.Application.UseCases.Auth
{
    using AutoMapper;
    using Eventer.Contracts.DTOs.Auth;
    using Eventer.Domain.Contracts.Auth;
    using FluentValidation;

    public class LoginUserUseCase : ILoginUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtProvider _jwtProvider;
        private readonly IValidator<LoginUserRequest> _validator;
        private readonly IMapper _mapper;

        public LoginUserUseCase(
            IUnitOfWork unitOfWork,
            IJwtProvider jwtProvider,
            IValidator<LoginUserRequest> validator,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _jwtProvider = jwtProvider;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<LoginResponse> Execute(LoginUserRequest request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var user = await _unitOfWork.Users.GetByUserNameAsync(request.UserName, cancellationToken);

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
