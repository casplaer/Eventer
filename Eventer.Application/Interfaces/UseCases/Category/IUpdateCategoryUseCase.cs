using Eventer.Domain.Contracts.Categories;

namespace Eventer.Application.Interfaces.UseCases.Category
{
    public interface IUpdateCategoryUseCase
    {
        Task ExecuteAsync(UpdateCategoryRequest request, CancellationToken cancellationToken);
    }

}
