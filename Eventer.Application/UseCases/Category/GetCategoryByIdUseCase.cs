using Eventer.Application.Interfaces.UseCases.Category;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;

namespace Eventer.Application.UseCases.Category
{
    public class GetCategoryByIdUseCase : IGetCategoryByIdUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoryByIdUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EventCategory?> ExecuteAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Categories.GetByIdAsync(id, cancellationToken);
        }
    }

}
