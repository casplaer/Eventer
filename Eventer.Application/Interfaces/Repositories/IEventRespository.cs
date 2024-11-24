using Eventer.Application.Contracts.Events;
using Eventer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventer.Application.Interfaces.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<IEnumerable<Event>> GetFilteredEventsAsync(GetEventsRequest request);
        IQueryable<Event> GetAllQueryable();
    }
}
