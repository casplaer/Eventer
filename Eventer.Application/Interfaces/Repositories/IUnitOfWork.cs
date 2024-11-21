using Eventer.Domain.Models;

namespace Eventer.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IEventRepository Events { get; }
        ICategoryRepository Categories { get; }
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync();
    }
}
