using Eventer.Domain.Models;

namespace Eventer.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByNormalizedEmailAsync(string normalizedEmail, CancellationToken cancellationToken);
        Task<User> GetByUserNameAsync(string userName, CancellationToken cancellationToken);
        Task<User> GetByRefreshTokenAsync(string token, CancellationToken cancellationToken);
    }
}
