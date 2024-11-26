using Eventer.Domain.Models;
using Eventer.Infrastructure.Data;
using Eventer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Eventer.Tests.RepositoryMethods
{
    public class GetByIdAsyncTests
    {
        [Fact]
        public async Task GetByIdAsync_ShouldReturnEvent_WhenEventExists()
        {
            var options = new DbContextOptionsBuilder<EventerDbContext>()
                .UseInMemoryDatabase(databaseName: "GetByIdTestDB")
                .Options;

            using var context = new EventerDbContext(options);

            var category = new EventCategory { Id = Guid.NewGuid(), Name = "Test Category" };
            var testEvent = new Event
            {
                Id = Guid.NewGuid(),
                Title = "Test Event",
                Description = "Description",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                StartTime = TimeOnly.FromDateTime(DateTime.Now),
                Venue = "Test Venue",
                MaxParticipants = 100,
                Category = category
            };

            await context.Categories.AddAsync(category);
            await context.Events.AddAsync(testEvent);
            await context.SaveChangesAsync();

            var repository = new EventRepository(context);

            var result = await repository.GetByIdAsync(testEvent.Id);

            Assert.NotNull(result);
            Assert.Equal(testEvent.Id, result.Id);
            Assert.Equal("Test Event", result.Title);
            Assert.NotNull(result.Category);
            Assert.Equal("Test Category", result.Category.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenEventDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<EventerDbContext>()
                .UseInMemoryDatabase(databaseName: "GetByIdNullTestDB")
                .Options;

            using var context = new EventerDbContext(options);
            var repository = new EventRepository(context);

            var result = await repository.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldIncludeCategoryAndRegistrations()
        {
            var options = new DbContextOptionsBuilder<EventerDbContext>()
                .UseInMemoryDatabase(databaseName: "GetByIdWithRelationsTestDB")
                .Options;

            using var context = new EventerDbContext(options);

            var category = new EventCategory { Id = Guid.NewGuid(), Name = "Test Category" };
            var testEvent = new Event
            {
                Id = Guid.NewGuid(),
                Title = "Test Event",
                Description = "Description",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                StartTime = TimeOnly.FromDateTime(DateTime.Now),
                Venue = "Test Venue",
                MaxParticipants = 100,
                Category = category,
                Registrations = new List<EventRegistration>
        {
            new EventRegistration { Id = Guid.NewGuid(), Name = "John", Surname = "Doe", Email = "john.doe@test.com" }
        }
            };

            await context.Categories.AddAsync(category);
            await context.Events.AddAsync(testEvent);
            await context.SaveChangesAsync();

            var repository = new EventRepository(context);

            var result = await repository.GetByIdAsync(testEvent.Id);

            Assert.NotNull(result);
            Assert.NotNull(result.Category);
            Assert.NotEmpty(result.Registrations);
            Assert.Equal("Test Category", result.Category.Name);
            Assert.Equal("John", result.Registrations.First().Name);
        }

    }
}
