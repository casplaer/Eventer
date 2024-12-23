using Eventer.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Eventer.Contracts.Requests.Events
{
    public record UpdateEventRequest(Guid Id,
        string? Title,
        string? Description,
        DateOnly? StartDate,
        TimeOnly? StartTime,
        string? Venue,
        double? Latitude,
        double? Longitude,
        EventCategory? Category,
        int? MaxParticipants,
        IEnumerable<IFormFile>? Images,
        IEnumerable<string>? RemovedImages,
        IEnumerable<string>? ExistingImages);
}
