﻿using Eventer.Domain.Contracts.Events;

namespace Eventer.Domain.Contracts.Events
{
    public record GetEventsResponse(List<EventDTO> Events, int TotalPages);
}
