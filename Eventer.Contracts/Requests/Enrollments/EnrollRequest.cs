namespace Eventer.Contracts.Requests.Enrollments
{
    public record EnrollRequest(
        Guid EventId,
        string Name,
        string SurName,
        string Email,
        DateOnly DateOfBirth
        );
}
