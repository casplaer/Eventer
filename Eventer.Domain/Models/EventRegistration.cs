namespace Eventer.Domain.Models
{
    public class EventRegistration
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public Guid EventId { get; set; }
        public Guid UserId { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
