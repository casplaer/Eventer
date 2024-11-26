namespace Eventer.Application.Contracts.Enrollments
{
    public record SingleEnrollmentResponse(
        Guid EnrollmentId,
        Guid EventId, 
        string Name,
        string Surname,
        string Email,
        DateOnly DateOfBirth
        );
}
