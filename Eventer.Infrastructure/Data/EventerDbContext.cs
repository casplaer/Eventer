using Eventer.Domain.Models;
using Eventer.Infrastructure.Configurations;
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
        public DbSet<EventRegistration> Registrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new EventCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new EventRegistrationConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
