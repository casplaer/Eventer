namespace Eventer.Domain.Contracts.Events
{
    public record GetEventsRequest(
        string? Title,
        DateOnly? Date,
        string? Venue,
        Guid? CategoryId,
        int Page
        );
}
