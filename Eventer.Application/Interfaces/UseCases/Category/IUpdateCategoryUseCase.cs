using Eventer.Application.Contracts.Categories;

namespace Eventer.Application.Interfaces.UseCases.Category
{
    public interface IUpdateCategoryUseCase
    {
        Task ExecuteAsync(UpdateCategoryRequest request, CancellationToken cancellationToken);
    }

}
