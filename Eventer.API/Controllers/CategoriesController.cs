using Eventer.Application.Contracts.Categories;
using Eventer.Application.Contracts.Events;
using Eventer.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Eventer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        public readonly ICategoryService _categoriesService;

        public CategoriesController(ICategoryService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery]string? name)
        {
            var categories = await _categoriesService.GetCategoriesByNameAsync(name);

            return Ok(new GetCategoriesResponse(categories));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody]CreateCategoryRequest request)
        {
            try
            {
                await _categoriesService.AddCategoryAsync(request);
                return Ok("Category successfully created.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the category. Message: {ex}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateCategoryRequest request)
        {
            await _categoriesService.UpdateCategoryAsync(request);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var isDeleted = await _categoriesService.DeleteCategoryAsync(id);

            if (!isDeleted)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            return NoContent();
        }
    }
}
