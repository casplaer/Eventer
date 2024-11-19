using Eventer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventer.Infrastructure.Data
{
    public class EventsDbContext : DbContext
    {

        public EventsDbContext(DbContextOptions<EventsDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventCategory> Categories { get; set; }
    }
}
