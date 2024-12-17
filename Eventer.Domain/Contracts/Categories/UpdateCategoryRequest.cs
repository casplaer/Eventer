namespace Eventer.Application.Contracts.Categories
{
    public record UpdateCategoryRequest(
        Guid Id,
        string? Name,
        string? Description
        );
}
