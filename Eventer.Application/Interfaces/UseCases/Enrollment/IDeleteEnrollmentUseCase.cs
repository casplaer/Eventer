namespace Eventer.Application.Interfaces.UseCases.Enrollment
{
    public interface IDeleteEnrollmentUseCase
    {
        Task<bool> ExecuteAsync(Guid id, CancellationToken cancellationToken);
    }
}
