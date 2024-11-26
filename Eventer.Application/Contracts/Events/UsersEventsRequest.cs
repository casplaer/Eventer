namespace Eventer.Application.Contracts.Events
{
    public record UsersEventsRequest(
        Guid UserId,
        int Page
        );
}
