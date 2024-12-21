namespace Eventer.Domain.Contracts.Enrollments
{
    public record UpdateEnrollRequest(
        Guid EnrollmentId,
        string Name,
        string Surname,
        string Email,
        DateOnly DateOfBirth
        );
}
