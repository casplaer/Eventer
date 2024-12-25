using Eventer.Domain.Enums;
using Eventer.Domain.Models;
using Eventer.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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

            modelBuilder.Entity<EventCategory>().HasData(
                new EventCategory
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Music",
                    Description = "Music related events"
                },
                new EventCategory
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Technology",
                    Description = "Tech conferences and workshops"
                },
                new EventCategory
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Art",
                    Description = "Art exhibitions and shows"
                }
            );
        }
    }
}
