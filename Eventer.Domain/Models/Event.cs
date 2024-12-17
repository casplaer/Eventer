namespace Eventer.Domain.Models
{
    public class Event
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public DateOnly StartDate { get; set; }
        public TimeOnly StartTime { get; set; } 
        
        public string Venue {  get; set; } = string.Empty; 
        
        public Guid CategoryId { get; set; }

        public EventCategory Category { get; set; } 

        public int MaxParticipants { get; set; } 
        public List<EventRegistration> Registrations { get; set; } = []; 

        public List<string>? ImageURLs { get; set; } 
    }
}
