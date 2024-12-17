namespace Eventer.Application.Interfaces.UseCases.Events
{
    public interface IDeleteEventUseCase
    {
        Task<bool> ExecuteAsync(Guid id, CancellationToken cancellationToken);
    }
}
