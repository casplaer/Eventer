using Eventer.Domain.Models;

namespace Eventer.Application.Interfaces.UseCases.Category
{
    public interface IGetAllCategoriesUseCase
    {
        Task<IEnumerable<EventCategory?>> ExecuteAsync(CancellationToken cancellationToken);
    }

}
