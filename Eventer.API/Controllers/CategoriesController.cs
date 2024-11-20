using Eventer.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Eventer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        public readonly IEventService _Service;

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {

            return Ok();
        }
        
    }
}
