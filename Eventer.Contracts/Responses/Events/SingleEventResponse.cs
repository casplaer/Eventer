using Eventer.Domain.Models;

namespace Eventer.Contracts.Responses.Events
{
    public record SingleEventResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateOnly StartDate { get; init; }
        public TimeOnly StartTime { get; init; }
        public string Venue { get; init; } = string.Empty;
        public EventCategory Category { get; init; }
        public int MaxParticipants { get; init; }
        public int CurrentRegistrations { get; init; }
        public IEnumerable<string>? Images { get; init; } = Enumerable.Empty<string>();
    }

}
