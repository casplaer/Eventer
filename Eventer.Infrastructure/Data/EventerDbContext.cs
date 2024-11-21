using Eventer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventer.Infrastructure.Data
{
    public class EventerDbContext : DbContext
    {

        public EventerDbContext(DbContextOptions<EventerDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventCategory> Categories { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
