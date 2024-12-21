using Eventer.Domain.Contracts.Categories;

namespace Eventer.Application.Interfaces.UseCases.Category
{
    public interface IAddCategoryUseCase
    {
        Task ExecuteAsync(CreateCategoryRequest request, CancellationToken cancellationToken);
    }

}
