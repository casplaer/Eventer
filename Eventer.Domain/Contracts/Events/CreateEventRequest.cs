﻿using Eventer.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Eventer.Domain.Contracts.Events
{
    public record CreateEventRequest(
        string Title,
        string Description,
        DateOnly StartDate,
        TimeOnly StartTime,
        string Venue,
        EventCategory Category,
        int MaxParticipants,
        IEnumerable<IFormFile>? Images);
}
