using Eventer.Application.Contracts.Enrollments;
using Eventer.Application.Contracts.Events;
using Eventer.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eventer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateEvent([FromForm]CreateEventRequest request)
        {
            await _eventService.AddEventAsync(request);

            return Ok();
        }

        [HttpGet("throw")]
        public IActionResult ThrowException()
        {
            throw new InvalidOperationException("Произошла ошибка!");
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetEvents([FromQuery]GetEventsRequest request)
        {
            var data = await _eventService.GetFilteredEventsAsync(request);

            if(data == null)
            {
                return Ok(new GetEventsResponse(new List<EventDTO>(), 1));
            }

            var eventDtos = data.Items
                                .Select(e => new EventDTO(e.Id, e.Title,
                                                          e.Description, e.StartDate,
                                                          e.StartTime, e.Venue,
                                                          e.Latitude, e.Longitude,
                                                          e.Category,
                                                          e.MaxParticipants,
                                                          e.ImageURLs,
                                                          e.Registrations.Count))
                                .ToList();

            var totalPages = data.TotalPages;

            return Ok(new GetEventsResponse(eventDtos, totalPages));
        }

        [HttpGet("your-events")]
        [Authorize]
        public async Task<IActionResult> GetUsersEvents([FromQuery] UsersEventsRequest request)
        {
            var data = await _eventService.GetUsersEventsAsync(request);

            var eventDtos = data.Items
                                .Select(e => new EventDTO(e.Id, e.Title,
                                                          e.Description, e.StartDate,
                                                          e.StartTime, e.Venue,
                                                          e.Latitude, e.Longitude,
                                                          e.Category,
                                                          e.MaxParticipants,
                                                          e.ImageURLs,
                                                          e.Registrations.Count))
                                .ToList();

            var totalPages = data.TotalPages;

            return Ok(new GetEventsResponse(eventDtos, totalPages));
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetEvent(Guid id)
        {
            var eventToReturn = await _eventService.GetEventByIdAsync(id);

            if(eventToReturn == null)
            {
                return NotFound(new { Message = "Событие не найдено." });
            }

            return Ok(eventToReturn);
        }

        [HttpPut]
        [Authorize(Policy = "AdminPolicy")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateEvent([FromForm]UpdateEventRequest request)
        {
            await _eventService.UpdateEventAsync(request);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var isDeleted = await _eventService.DeleteEventAsync(id);

            if (!isDeleted)
            {
                return NotFound($"Event with ID {id} not found.");
            }

            return NoContent();
        }

        [HttpPost("enroll")]
        [Authorize]
        public async Task<IActionResult> EnrollOnEvent([FromBody] EnrollRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _eventService.EnrollOnEventAsync(request, new Guid(userId!));
            return Ok();
        }

        [HttpGet("{eventId}/isEnrolled")]
        [Authorize]
        public async Task<IActionResult> IsUserEnrolled(Guid eventId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var enrollmentId = await _eventService.IsUserEnrolledAsync(eventId, Guid.Parse(userId));
                return Ok(new { IsEnrolled = true,
                                Id = enrollmentId});
            }
            catch(Exception ex)
            {
                return Ok(new {IsEnrolled = false});
            }
        }

        [HttpPut("enrollment/edit")]
        [Authorize]
        public async Task<IActionResult> UpdateEnrollment([FromBody]UpdateEnrollRequest request)
        {
            try
            {
                await _eventService.UpdateEnrollmentAsync(request);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message = "Что-то пошло не так..." });
            }

        }

        [HttpGet("enrollment/{id}")]
        [Authorize]
        public async Task<IActionResult> GetSingleEnrollment(Guid id)
        {
            var enrollment = await _eventService.GetSingleEnrollmentById(id);

            return Ok(enrollment);
        }

        [HttpDelete("enrollment/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteEnrollment(Guid id)
        {
            var result = await _eventService.DeleteEnrollmentAsync(id);
            
            if(!result)
            {
                return BadRequest(new { Message = "Не удалось удалить запись." });
            }
                
            return NoContent();
        }
    }
}
