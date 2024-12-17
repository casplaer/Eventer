using Eventer.Application.Interfaces.UseCases.Events;
using Eventer.Domain.Contracts.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private readonly IAddEventUseCase _addEventUseCase;
        private readonly IGetFilteredEventsUseCase _getFilteredEventsUseCase;
        private readonly IGetUsersEventsUseCase _getUsersEventsUseCase;
        private readonly IGetEventByIdUseCase _getEventByIdUseCase;
        private readonly IUpdateEventUseCase _updateEventUseCase;
        private readonly IDeleteEventUseCase _deleteEventUseCase;

        public EventsController(
            IAddEventUseCase addEventUseCase,
            IGetFilteredEventsUseCase getFilteredEventsUseCase,
            IGetUsersEventsUseCase getUsersEventsUseCase,
            IGetEventByIdUseCase getEventByIdUseCase,
            IUpdateEventUseCase updateEventUseCase,
            IDeleteEventUseCase deleteEventUseCase)
        {
            _addEventUseCase = addEventUseCase;
            _getFilteredEventsUseCase = getFilteredEventsUseCase;
            _getUsersEventsUseCase = getUsersEventsUseCase;
            _getEventByIdUseCase = getEventByIdUseCase;
            _updateEventUseCase = updateEventUseCase;
            _deleteEventUseCase = deleteEventUseCase;
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateEvent([FromForm] CreateEventRequest request, CancellationToken cancellationToken)
        {
            await _addEventUseCase.ExecuteAsync(request, cancellationToken);
            return Ok(new { Message = "Событие создано успешно." });
        }

        [HttpGet("throw")]
        public IActionResult ThrowException()
        {
            throw new InvalidOperationException("Произошла ошибка!");
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents([FromQuery] GetEventsRequest request, CancellationToken cancellationToken)
        {
            var data = await _getFilteredEventsUseCase.ExecuteAsync(request, cancellationToken);

            var eventDtos = data.Items.Select(e => new EventDTO(
                e.Id,
                e.Title,
                e.Description,
                e.StartDate,
                e.StartTime,
                e.Venue,
                e.Category,
                e.MaxParticipants,
                e.ImageURLs,
                e.Registrations.Count)).ToList();

            return Ok(new GetEventsResponse(eventDtos, data.TotalPages));
        }

        [HttpGet("your-events")]
        public async Task<IActionResult> GetUsersEvents([FromQuery] UsersEventsRequest request, CancellationToken cancellationToken)
        {
            var data = await _getUsersEventsUseCase.ExecuteAsync(request, cancellationToken);

            var eventDtos = data.Items.Select(e => new EventDTO(
                e.Id,
                e.Title,
                e.Description,
                e.StartDate,
                e.StartTime,
                e.Venue,
                e.Category,
                e.MaxParticipants,
                e.ImageURLs,
                e.Registrations.Count)).ToList();

            return Ok(new GetEventsResponse(eventDtos, data.TotalPages));
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetEvent(Guid id, CancellationToken cancellationToken)
        {
            var eventToReturn = await _getEventByIdUseCase.ExecuteAsync(id, cancellationToken);
            return Ok(eventToReturn);
        }

        [HttpPut]
        [Authorize(Policy = "AdminPolicy")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateEvent([FromForm] UpdateEventRequest request, CancellationToken cancellationToken)
        {
            await _updateEventUseCase.ExecuteAsync(request, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteEvent(Guid id, CancellationToken cancellationToken)
        {
            await _deleteEventUseCase.ExecuteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
