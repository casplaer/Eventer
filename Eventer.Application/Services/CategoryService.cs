using Eventer.Application.Contracts.Categories;
using Eventer.Application.Interfaces.Repositories;
using Eventer.Application.Interfaces.Services;
using Eventer.Domain.Models;

namespace Eventer.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddCategoryAsync(CreateCategoryRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            }

            var existingCategory = await _unitOfWork.Categories.GetByNameAsync(request.Name);
            if (existingCategory.Any())
            {
                throw new InvalidOperationException($"Category with name '{request.Name}' already exists.");
            }

            var newCategory = new EventCategory
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Description = request.Description,
            };

            await _unitOfWork.Categories.AddAsync(newCategory);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteCategoryAsync(Guid Id)
        {
            var categoryToDelete = await _unitOfWork.Categories.GetByIdAsync(Id);

            if (categoryToDelete == null)
                return false;


            await _unitOfWork.Categories.DeleteAsync(Id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<EventCategory>> GetAllCategoriesAsync()
        {
            return await _unitOfWork.Categories.GetAllAsync();
        }

        public async Task<EventCategory?> GetCategoryByIdAsync(Guid Id)
        {
            return await _unitOfWork.Categories.GetByIdAsync(Id);
        }

        public async Task<IEnumerable<EventCategory?>> GetCategoriesByNameAsync(string Name)
        {
            return await _unitOfWork.Categories.GetByNameAsync(Name);
        }

        public async Task UpdateCategoryAsync(UpdateCategoryRequest request)
        {
            var categoryToUpdate = await _unitOfWork.Categories.GetByIdAsync(request.Id);

            if(categoryToUpdate == null)
            {
                throw new ArgumentException($"Category with ID {request.Id} not found");
            }

            if(!string.IsNullOrEmpty(request.Name))
                categoryToUpdate.Name = request.Name;

            if(!string.IsNullOrEmpty(request.Description))
                categoryToUpdate.Description = request.Description;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
