namespace Eventer.Application.Contracts.Categories
{
    public record CreateCategoryRequest(
        string Name,
        string? Description
        );
}
