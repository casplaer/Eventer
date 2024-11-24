using Eventer.Domain.Models;

namespace Eventer.Application.Contracts.Events
{
    public record EventDTO(
        Guid Id,
        string Title,
        string Description,
        DateOnly StartDate,
        TimeOnly StartTime,
        string Venue,
        double Latitude,
        double Longitude,
        EventCategory Category,
        int MaxParticipants,
        List<string>? ImageURLs);
}
