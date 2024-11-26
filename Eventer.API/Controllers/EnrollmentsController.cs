using Eventer.Application.Contracts.Enrollments;
using Eventer.Application.Interfaces.Services;
using Eventer.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eventer.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EnrollOnEvent([FromBody] EnrollRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _enrollmentService.EnrollOnEventAsync(request, new Guid(userId!));
            return Ok();
        }

        [HttpGet("{eventId}/isEnrolled")]
        [Authorize]
        public async Task<IActionResult> IsUserEnrolled(Guid eventId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var enrollmentId = await _enrollmentService.IsUserEnrolledAsync(eventId, Guid.Parse(userId));
                return Ok(new
                {
                    IsEnrolled = true,
                    Id = enrollmentId
                });
            }
            catch (Exception ex)
            {
                return Ok(new { IsEnrolled = false });
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateEnrollment([FromBody] UpdateEnrollRequest request)
        {
            try
            {
                await _enrollmentService.UpdateEnrollmentAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Что-то пошло не так..." });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetSingleEnrollment(Guid id)
        {
            var enrollment = await _enrollmentService.GetSingleEnrollmentById(id);

            return Ok(enrollment);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteEnrollment(Guid id)
        {
            var result = await _enrollmentService.DeleteEnrollmentAsync(id);

            if (!result)
            {
                return BadRequest(new { Message = "Не удалось удалить запись." });
            }

            return NoContent();
        }

        [HttpGet("users-enrolled/{id}")]
        [Authorize]
        public async Task<IActionResult> GetAllUsersEnrolled(Guid id)
        {
            try
            {
                var users = await _enrollmentService.GetAllEnrollmentsAsync(id);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }
    }
}
