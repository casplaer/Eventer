using Eventer.Application.Contracts.Events;
using Eventer.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eventer.API.Controllers
{
    [Authorize]
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetEvents([FromQuery]GetEventsRequest request)
        {
            var data = await _eventService.GetFilteredEventsAsync(request);

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

        [HttpPost("enroll")]
        [Authorize]
        public async Task<IActionResult> EnrollOnEvent([FromBody] EnrollRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _eventService.EnrollOnEventAsync(request, new Guid(userId!));
            return Ok();
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
    }
}
