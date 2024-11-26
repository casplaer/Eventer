﻿namespace Eventer.Application.Contracts.Enrollments
{
    public record UpdateEnrollRequest(
        Guid EnrollmentId,
        string Name,
        string Surname,
        DateOnly DateOfBirth
        );
}
