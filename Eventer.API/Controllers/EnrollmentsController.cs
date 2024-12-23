using Eventer.Application.Interfaces.UseCases.Enrollment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Eventer.Contracts.Requests.Enrollments;

namespace Eventer.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : Controller
    {
        private readonly IEnrollOnEventUseCase _enrollOnEventUseCase;
        private readonly ICheckUserEnrollmentUseCase _checkUserEnrollmentUseCase;
        private readonly IUpdateEnrollmentUseCase _updateEnrollmentUseCase;
        private readonly IGetSingleEnrollmentByIdUseCase _getSingleEnrollmentUseCase;
        private readonly IDeleteEnrollmentUseCase _deleteEnrollmentUseCase;
        private readonly IGetAllEnrollmentsUseCase _getAllEnrollmentsUseCase;

        public EnrollmentsController(
            IEnrollOnEventUseCase enrollOnEventUseCase,
            ICheckUserEnrollmentUseCase checkUserEnrollmentUseCase,
            IUpdateEnrollmentUseCase updateEnrollmentUseCase,
            IGetSingleEnrollmentByIdUseCase getSingleEnrollmentUseCase,
            IDeleteEnrollmentUseCase deleteEnrollmentUseCase,
            IGetAllEnrollmentsUseCase getAllEnrollmentsUseCase)
        {
            _enrollOnEventUseCase = enrollOnEventUseCase;
            _checkUserEnrollmentUseCase = checkUserEnrollmentUseCase;
            _updateEnrollmentUseCase = updateEnrollmentUseCase;
            _getSingleEnrollmentUseCase = getSingleEnrollmentUseCase;
            _deleteEnrollmentUseCase = deleteEnrollmentUseCase;
            _getAllEnrollmentsUseCase = getAllEnrollmentsUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> EnrollOnEvent([FromBody] EnrollRequest request, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _enrollOnEventUseCase.ExecuteAsync(request, Guid.Parse(userId!), cancellationToken);
            return Ok(new { Message = "Enrollment successful." });
        }

        [HttpGet("{eventId}/isEnrolled")]
        public async Task<IActionResult> IsUserEnrolled(Guid eventId, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var enrollmentResponse = await _checkUserEnrollmentUseCase.ExecuteAsync(eventId, Guid.Parse(userId), cancellationToken);
            return Ok(enrollmentResponse);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEnrollment([FromBody] UpdateEnrollRequest request, CancellationToken cancellationToken)
        {
            await _updateEnrollmentUseCase.ExecuteAsync(request, cancellationToken);
            return Ok(new { Message = "Enrollment updated successfully." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleEnrollment(Guid id, CancellationToken cancellationToken)
        {
            var enrollment = await _getSingleEnrollmentUseCase.ExecuteAsync(id, cancellationToken);
            return Ok(enrollment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(Guid id, CancellationToken cancellationToken)
        {
            await _deleteEnrollmentUseCase.ExecuteAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpGet("users-enrolled/{id}")]
        public async Task<IActionResult> GetAllUsersEnrolled(Guid id, CancellationToken cancellationToken)
        {
            var users = await _getAllEnrollmentsUseCase.ExecuteAsync(id, cancellationToken);
            return Ok(users);
        }
    }
}
