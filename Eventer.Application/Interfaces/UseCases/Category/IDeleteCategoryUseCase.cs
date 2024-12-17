namespace Eventer.Application.Interfaces.UseCases.Category
{
    public interface IDeleteCategoryUseCase
    {
        Task<bool> ExecuteAsync(Guid id, CancellationToken cancellationToken);
    }

}
