using Eventer.Domain.Models;

namespace Eventer.Application.Interfaces.UseCases.Category
{
    public interface IGetCategoriesByNameUseCase
    {
        Task<IEnumerable<EventCategory?>> ExecuteAsync(string name, CancellationToken cancellationToken);
    }

}
