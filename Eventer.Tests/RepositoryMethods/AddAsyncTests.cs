using Eventer.Domain.Models;
using Eventer.Infrastructure.Data;
using Eventer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventer.Tests.RepositoryMethods
{
    public class AddAsyncTests
    {
        [Fact]
        public async Task AddAsync_ShouldAddEventToDatabase()
        {
            var options = new DbContextOptionsBuilder<EventerDbContext>()
                .UseInMemoryDatabase(databaseName: "AddEventTestDB")
                .Options;

            using var context = new EventerDbContext(options);

            var testEvent = new Event
            {
                Id = Guid.NewGuid(),
                Title = "New Event",
                Description = "Event Description",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                StartTime = TimeOnly.FromDateTime(DateTime.Now),
                Venue = "New Venue",
                MaxParticipants = 200
            };

            var repository = new EventRepository(context);

            await repository.AddAsync(testEvent);
            await context.SaveChangesAsync();

            var addedEvent = await context.Events.FirstOrDefaultAsync(e => e.Id == testEvent.Id);
            Assert.NotNull(addedEvent);
            Assert.Equal("New Event", addedEvent.Title);
            Assert.Equal(200, addedEvent.MaxParticipants);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEventWithCategory()
        {
            var options = new DbContextOptionsBuilder<EventerDbContext>()
                .UseInMemoryDatabase(databaseName: "AddEventWithCategoryTestDB")
                .Options;

            using var context = new EventerDbContext(options);

            var category = new EventCategory { Id = Guid.NewGuid(), Name = "Test Category" };
            var testEvent = new Event
            {
                Id = Guid.NewGuid(),
                Title = "New Event",
                Description = "Event Description",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                StartTime = TimeOnly.FromDateTime(DateTime.Now),
                Venue = "New Venue",
                MaxParticipants = 200,
                Category = category
            };

            var repository = new EventRepository(context);

            await context.Categories.AddAsync(category);
            await repository.AddAsync(testEvent);
            await context.SaveChangesAsync();

            var addedEvent = await context.Events.Include(e => e.Category).FirstOrDefaultAsync(e => e.Id == testEvent.Id);
            Assert.NotNull(addedEvent);
            Assert.NotNull(addedEvent.Category);
            Assert.Equal("Test Category", addedEvent.Category.Name);
        }

    }
}
