using Eventer.Application.Interfaces.UseCases.Category;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;

namespace Eventer.Application.UseCases.Category
{
    public class GetCategoriesByNameUseCase : IGetCategoriesByNameUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoriesByNameUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EventCategory?>> ExecuteAsync(string name, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Categories.GetByNameAsync(name, cancellationToken);
        }
    }

}
