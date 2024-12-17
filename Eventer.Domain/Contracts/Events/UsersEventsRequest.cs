namespace Eventer.Domain.Contracts.Events
{
    public record UsersEventsRequest(
        Guid UserId,
        int Page
        );
}
