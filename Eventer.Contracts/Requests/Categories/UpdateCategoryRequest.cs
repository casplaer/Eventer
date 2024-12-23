namespace Eventer.Contracts.Requests.Categories
{
    public record UpdateCategoryRequest(
        Guid Id,
        string? Name,
        string? Description
        );
}
