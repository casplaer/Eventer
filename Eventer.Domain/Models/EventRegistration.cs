using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Event Event { get; set; } //Событие

        public DateTime RegistrationDate { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;
    }
}
