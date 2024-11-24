using Eventer.Domain.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

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
        IEnumerable<IFormFile>? Images);
}
