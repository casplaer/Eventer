namespace Eventer.Domain.Contracts.Categories
{
    public record CreateCategoryRequest(
        string Name,
        string? Description
        );
}
