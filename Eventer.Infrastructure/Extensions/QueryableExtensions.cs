using System.Linq.Expressions;

namespace Eventer.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, Expression<Func<T, bool>> filter, bool condition)
        {
            return condition ? query.Where(filter) : query;
        }
    }
}
