namespace Eventer.Application.Interfaces
{
    public interface IUniqueFieldChecker
    {
        Task<bool> IsUniqueAsync<TEntity>(string fieldName, string value)
            where TEntity : class;
    }
}
