using Eventer.Domain.Models;

namespace Eventer.Application.Interfaces.UseCases.Auth
{
    public interface IGetUserByUsernameUseCase
    {
        Task<User> Execute(string username, CancellationToken cancellationToken);
    }
}
