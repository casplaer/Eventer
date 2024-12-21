using Eventer.Domain.Contracts.Auth;

namespace Eventer.Application.Interfaces.UseCases.Auth
{
    public interface ILoginUserUseCase
    {
        Task<LoginResponse> Execute(LoginUserRequest request, CancellationToken cancellationToken);
    }
}
