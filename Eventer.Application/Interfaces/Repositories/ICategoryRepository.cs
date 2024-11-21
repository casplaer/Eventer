using Eventer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventer.Application.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepository<EventCategory>
    {
        Task<IEnumerable<EventCategory?>> GetByNameAsync(string name);
    }
}
