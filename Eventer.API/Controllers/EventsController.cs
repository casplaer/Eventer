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
        public async Task<IActionResult> CreateEvent([FromBody]CreateEventRequest request)
        {
            await _eventService.AddEventAsync(request);

            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetEvents([FromQuery]GetEventsRequest request)
        {
            var eventDtos = (await _eventService.GetFilteredEventsAsync(request))
                                .Select(e => new EventDTO(e.Id, e.Title,
                                                          e.Description, e.StartDate,
                                                          e.StartTime, e.Venue,
                                                          e.Latitude, e.Longitude,
                                                          e.Category,
                                                          e.MaxParticipants,
                                                          e.ImageURLs))
                                .ToList();


            return Ok(new GetEventsResponse(eventDtos));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEvent([FromBody]UpdateEventRequest request)
        {
            await _eventService.UpdateEventAsync(request);

            return NoContent();
        }

        [HttpDelete("{id}")]
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
