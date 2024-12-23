namespace Eventer.Contracts.Requests.Events
{
    public record UsersEventsRequest(
        Guid UserId,
        int Page
        );
}
