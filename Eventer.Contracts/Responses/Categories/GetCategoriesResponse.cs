using Eventer.Domain.Models;

namespace Eventer.Contracts.Responses.Categories
{
    public record GetCategoriesResponse(IEnumerable<EventCategory> Categories);
}
