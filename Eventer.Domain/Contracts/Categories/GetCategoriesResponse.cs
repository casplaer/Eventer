using Eventer.Domain.Models;

namespace Eventer.Application.Contracts.Events
{
    public record GetCategoriesResponse(IEnumerable<EventCategory> Categories);
}
