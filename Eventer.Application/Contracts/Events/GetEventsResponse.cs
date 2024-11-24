namespace Eventer.Application.Contracts.Events
{
    public record GetEventsResponse(List<EventDTO> Events, int TotalPages);
}
