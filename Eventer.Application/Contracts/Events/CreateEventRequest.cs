using Eventer.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Eventer.Application.Contracts.Events
{
    public record CreateEventRequest(
         [StringLength(40, MinimumLength = 5, ErrorMessage = "Некорректная длина названия.")] string Title,
        string Description,
        DateOnly StartDate,
        TimeOnly StartTime,
        string Venue,
        double Latitude,
        double Longitude,
        EventCategory Category,
        int MaxParticipants,
        IEnumerable<string>? ImageURLs);
}
