using Eventer.Application.Contracts.Categories;
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
        Task<IEnumerable<EventCategory?>> GetAllCategoriesAsync();
        Task<EventCategory?> GetCategoryByIdAsync(Guid Id);
        Task<IEnumerable<EventCategory?>> GetCategoriesByNameAsync(string Name);
        Task AddCategoryAsync(CreateCategoryRequest request);
        Task UpdateCategoryAsync(UpdateCategoryRequest request);
        Task<bool> DeleteCategoryAsync(Guid Id);
    }
}
