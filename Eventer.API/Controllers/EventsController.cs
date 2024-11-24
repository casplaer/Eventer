using Eventer.Application.Contracts;
using Eventer.Application.Contracts.Events;
using Eventer.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> CreateEvent([FromBody]CreateEventRequest request)
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
                                                          e.ImageURLs))
                                .ToList();

            var totalPages = data.TotalPages;

            return Ok(new GetEventsResponse(eventDtos, totalPages));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetEvent(Guid id)
        {
            var eventToReturn = await _eventService.GetEventByIdAsync(id);

            return Ok(eventToReturn);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateEvent([FromBody]UpdateEventRequest request)
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
