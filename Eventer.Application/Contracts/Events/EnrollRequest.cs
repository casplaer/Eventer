namespace Eventer.Application.Contracts.Events
{
    public record EnrollRequest(
        Guid EventId,
        string Name,
        string SurName,
        string Email,
        DateOnly DateOfBirth
        );
}
