using Eventer.Domain.Models;

namespace Eventer.Application.Interfaces.UseCases.Auth
{
    public interface IGetUserByTokenUseCase
    {
        Task<User> Execute(string refreshToken, CancellationToken cancellationToken);
    }
}
