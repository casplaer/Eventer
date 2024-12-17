using Eventer.Application.Contracts.Categories;
using Eventer.Application.Interfaces.UseCases.Category;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;
using FluentValidation;

namespace Eventer.Application.UseCases.Category
{
    public class AddCategoryUseCase : IAddCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<EventCategory> _validator;

        public AddCategoryUseCase(IUnitOfWork unitOfWork,
                IValidator<EventCategory> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task ExecuteAsync(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            }

            var existingCategory = await _unitOfWork.Categories.GetByNameAsync(request.Name, cancellationToken);
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

            var validationResult = await _validator.ValidateAsync(newCategory, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _unitOfWork.Categories.AddAsync(newCategory, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}
