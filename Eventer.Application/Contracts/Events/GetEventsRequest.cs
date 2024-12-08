﻿using Eventer.Domain.Models;

namespace Eventer.Application.Contracts.Events
{
    public record GetEventsRequest(
        string? Title,
        DateOnly? Date,
        string? Venue,
        Guid? CategoryId,
        int Page
        );
}
