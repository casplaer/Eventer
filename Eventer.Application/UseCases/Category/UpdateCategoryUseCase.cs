using AutoMapper;
using Eventer.Application.Contracts.Categories;
using Eventer.Application.Interfaces.UseCases.Category;
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

        public UpdateCategoryUseCase(IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<EventCategory> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task ExecuteAsync(UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            var categoryToUpdate = await _unitOfWork.Categories.GetByIdAsync(request.Id, cancellationToken);
            if (categoryToUpdate == null)
            {
                throw new ArgumentException($"Category with ID {request.Id} not found");
            }

            _mapper.Map(request, categoryToUpdate);

            var validationResult = await _validator.ValidateAsync(categoryToUpdate, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }

}
