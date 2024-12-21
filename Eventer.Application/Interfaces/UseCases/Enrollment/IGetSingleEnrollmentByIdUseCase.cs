﻿using Eventer.Domain.Contracts.Enrollments;

namespace Eventer.Application.Interfaces.UseCases.Enrollment
{
    public interface IGetSingleEnrollmentByIdUseCase
    {
        Task<SingleEnrollmentResponse?> ExecuteAsync(Guid id, CancellationToken cancellationToken);
    }
}
