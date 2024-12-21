namespace Eventer.Domain.Contracts.Enrollments
{
    public record EnrollRequest(
        Guid EventId,
        string Name,
        string SurName,
        string Email,
        DateOnly DateOfBirth
        );
}
