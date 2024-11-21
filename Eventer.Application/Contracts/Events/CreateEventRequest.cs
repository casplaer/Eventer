using Eventer.Domain.Models;

namespace Eventer.Application.Contracts.Events
{
    public record CreateEventRequest(
        string Title,
        string Description,
        DateOnly StartDate,
        TimeOnly StartTime,
        string Venue,
        double Latitude,
        double Longitude,
        EventCategory Category,
        int MaxParticipants);
}
