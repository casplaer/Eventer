using Eventer.Application.Contracts;
using Eventer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventer.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<EventCategory>> GetAllCategoriesAsync();
        Task<EventCategory> GetCategoryByIdAsync(Guid Id);
        Task<EventCategory> GetCategoryByNameAsync(string Name);
        Task AddCategoryAsync(CreateCategoryRequest request);
    }
}
