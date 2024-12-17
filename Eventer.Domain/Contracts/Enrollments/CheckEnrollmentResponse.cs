namespace Eventer.Domain.Contracts.Enrollments
{
    public record CheckEnrollmentResponse(
        bool IsEnrolled,
        Guid Id
        );
}
