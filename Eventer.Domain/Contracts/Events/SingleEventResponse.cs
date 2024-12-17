using Eventer.Domain.Models;

namespace Eventer.Domain.Contracts.Events
{
    public record SingleEventResponse(
        Guid Id,
        string Title,
        string Description,
        DateOnly StartDate,
        TimeOnly StartTime,
        string Venue,
        EventCategory Category,
        int MaxParticipants,
        int CurrentRegistrations,
        IEnumerable<string>? Images);
}
