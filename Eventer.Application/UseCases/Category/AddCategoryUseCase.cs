using AutoMapper;
using Eventer.Application.Exceptions;
using Eventer.Application.Interfaces;
using Eventer.Application.Interfaces.UseCases.Category;
using Eventer.Contracts.Requests.Categories;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;
using FluentValidation;

namespace Eventer.Application.UseCases.Category
{
    public class AddCategoryUseCase : IAddCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<EventCategory> _validator;
        private readonly IMapper _mapper;
        private readonly IUniqueFieldChecker _uniqueFieldChecker;

        public AddCategoryUseCase(
            IUnitOfWork unitOfWork,
            IValidator<EventCategory> validator,
            IMapper mapper,
            IUniqueFieldChecker uniqueFieldChecker)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _mapper = mapper;
            _uniqueFieldChecker = uniqueFieldChecker;
        }

        public async Task ExecuteAsync(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            var categoryToValidate = _mapper.Map<EventCategory>(request);

            if (!await _uniqueFieldChecker.IsUniqueAsync<EventCategory>("Name", request.Name))
            {
                throw new AlreadyExistsException("Категория с таким названием уже существует.");
            }

            await _validator.ValidateAndThrowAsync(categoryToValidate, cancellationToken);

            var newCategory = new EventCategory
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Description = request.Description,
            };

            await _unitOfWork.Categories.AddAsync(newCategory, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}
