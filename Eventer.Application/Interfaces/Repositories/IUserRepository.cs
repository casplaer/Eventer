using Eventer.Domain.Models;

namespace Eventer.Application.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByUserNameAsync(string userName);
        Task<User> GetByRefreshTokenAsync(string token);
    }
}
