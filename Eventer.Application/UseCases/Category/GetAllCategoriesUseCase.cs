using Eventer.Application.Interfaces.UseCases.Category;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;

namespace Eventer.Application.UseCases.Category
{
    public class GetAllCategoriesUseCase : IGetAllCategoriesUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCategoriesUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EventCategory>> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await _unitOfWork.Categories.GetAllAsync(cancellationToken);
        }
    }
}
