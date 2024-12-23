namespace Eventer.Contracts.Responses.Enrollments
{
    public record CheckEnrollmentResponse(
        bool IsEnrolled,
        Guid Id
        );
}
