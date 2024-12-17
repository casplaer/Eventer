using Eventer.Domain.Models;

namespace Eventer.Domain.Contracts.Events
{
    public record EventDTO(
        Guid Id,
        string Title,
        string Description,
        DateOnly StartDate,
        TimeOnly StartTime,
        string Venue,
        EventCategory Category,
        int MaxParticipants,
        List<string>? ImageURLs,
        int CurrentRegistrations);
}
