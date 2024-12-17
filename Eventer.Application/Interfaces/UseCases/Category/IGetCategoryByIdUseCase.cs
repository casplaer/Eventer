using Eventer.Domain.Models;

namespace Eventer.Application.Interfaces.UseCases.Category
{
    public interface IGetCategoryByIdUseCase
    {
        Task<EventCategory?> ExecuteAsync(Guid id, CancellationToken cancellationToken);
    }

}
