using Eventer.Contracts.Requests.Auth;
using Eventer.Contracts.Responses.Auth;

namespace Eventer.Application.Interfaces.UseCases.Auth
{
    public interface ILoginUserUseCase
    {
        Task<LoginResponse> Execute(LoginUserRequest request, CancellationToken cancellationToken);
    }
}
