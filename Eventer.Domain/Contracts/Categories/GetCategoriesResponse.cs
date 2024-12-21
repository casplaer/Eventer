using Eventer.Domain.Models;

namespace Eventer.Domain.Contracts.Categories
{
    public record GetCategoriesResponse(IEnumerable<EventCategory> Categories);
}
