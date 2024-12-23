namespace Eventer.Contracts.Requests.Categories
{
    public record CreateCategoryRequest(
        string Name,
        string? Description
        );
}
