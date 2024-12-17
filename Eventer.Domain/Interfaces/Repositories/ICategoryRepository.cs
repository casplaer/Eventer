using Eventer.Domain.Models;

namespace Eventer.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepository<EventCategory>
    {
        Task<IEnumerable<EventCategory?>> GetByNameAsync(string name, CancellationToken cancellationToken);
    }
}
