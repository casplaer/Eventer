using Eventer.Application.Contracts.Categories;
using Eventer.Application.Contracts.Events;
using Eventer.Application.Interfaces.UseCases.Category;
using Microsoft.AspNetCore.Mvc;

namespace Eventer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly IGetCategoriesByNameUseCase _getCategoriesByNameUseCase;
        private readonly IAddCategoryUseCase _addCategoryUseCase;
        private readonly IUpdateCategoryUseCase _updateCategoryUseCase;
        private readonly IDeleteCategoryUseCase _deleteCategoryUseCase;

        public CategoriesController(
            IGetCategoriesByNameUseCase getCategoriesByNameUseCase,
            IAddCategoryUseCase addCategoryUseCase,
            IUpdateCategoryUseCase updateCategoryUseCase,
            IDeleteCategoryUseCase deleteCategoryUseCase)
        {
            _getCategoriesByNameUseCase = getCategoriesByNameUseCase;
            _addCategoryUseCase = addCategoryUseCase;
            _updateCategoryUseCase = updateCategoryUseCase;
            _deleteCategoryUseCase = deleteCategoryUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] string? name, CancellationToken cancellationToken)
        {
            var categories = await _getCategoriesByNameUseCase.ExecuteAsync(name, cancellationToken);
            return Ok(new GetCategoriesResponse(categories));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            await _addCategoryUseCase.ExecuteAsync(request, cancellationToken);
            return Ok("Category successfully created.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            await _updateCategoryUseCase.ExecuteAsync(request, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken)
        {
            await _deleteCategoryUseCase.ExecuteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
