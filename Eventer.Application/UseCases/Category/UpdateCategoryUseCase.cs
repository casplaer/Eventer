using AutoMapper;
using Eventer.Application.Exceptions;
using Eventer.Application.Interfaces.UseCases.Category;
using Eventer.Contracts.Requests.Categories;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;
using FluentValidation;

namespace Eventer.Application.UseCases.Category
{
    public class UpdateCategoryUseCase : IUpdateCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<EventCategory> _validator;

        public UpdateCategoryUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<EventCategory> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task ExecuteAsync(UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            var categoryToValidate = _mapper.Map<EventCategory>(request);

            await _validator.ValidateAndThrowAsync(categoryToValidate, cancellationToken);

            var categoryToUpdate = await _unitOfWork.Categories.GetByIdAsync(request.Id, cancellationToken);
            if (categoryToUpdate == null)
            {
                throw new NotFoundException($"Category with ID {request.Id} not found.");
            }

            _mapper.Map(request, categoryToUpdate);

            await _unitOfWork.SaveChangesAsync();
        }
    }

}
