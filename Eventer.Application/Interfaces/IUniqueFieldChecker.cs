namespace Eventer.Application.Interfaces
{
    public interface IUniqueFieldChecker
    {
        Task<bool> IsUniqueAsync<TEntity>(string fieldName, string value, Guid? excludeId = null)
            where TEntity : class;
    }
}
