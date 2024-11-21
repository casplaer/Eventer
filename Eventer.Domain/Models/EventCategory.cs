using System.ComponentModel.DataAnnotations;

namespace Eventer.Domain.Models
{
    public class EventCategory
    {
        public Guid Id { get; set; }

        [StringLength(40, MinimumLength = 5)]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
