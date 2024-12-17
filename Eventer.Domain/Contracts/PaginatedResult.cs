namespace Eventer.Domain.Contracts
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public int TotalCount { get; set; } 
        public int TotalPages { get; set; } 
    }

}
