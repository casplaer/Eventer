namespace Eventer.Contracts.Requests.Enrollments
{
    public record UpdateEnrollRequest(
        Guid EnrollmentId,
        string Name,
        string Surname,
        string Email,
        DateOnly DateOfBirth
        );
}
