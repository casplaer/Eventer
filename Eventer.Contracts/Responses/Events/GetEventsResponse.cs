using Eventer.Contracts.DTOs.Events;

namespace Eventer.Contracts.Responses.Events
{
    public record GetEventsResponse(List<EventDTO> Events, int TotalPages);
}
