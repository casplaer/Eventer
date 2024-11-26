using Eventer.Application.Contracts.Events;
using Eventer.Application.Services;
using Eventer.Domain.Models;
using Eventer.Infrastructure.Data;
using Eventer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class EventServiceTests
{
    private EventerDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<EventerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new EventerDbContext(options);
    }

    [Fact]
    public async Task GetFilteredEventsAsync_ShouldReturnFilteredEventsByTitle()
    {
        var context = CreateInMemoryContext();
        var unitOfWork = new UnitOfWork(context);
        var service = new EventService(unitOfWork, new HttpClient());

        var event1 = new Event
        {
            Id = Guid.NewGuid(),
            Title = "Music Festival",
            Venue = "Arena",
            StartDate = new DateOnly(2024, 1, 1),
            Category = new EventCategory { Id = Guid.NewGuid(), Name = "Test Category" }
        };
        var event2 = new Event
        {
            Id = Guid.NewGuid(),
            Title = "Tech Conference",
            Venue = "Convention Center",
            StartDate = new DateOnly(2024, 2, 1),
            Category = new EventCategory { Id = Guid.NewGuid(), Name = "Test Category" }
        };

        await context.Events.AddRangeAsync(event1, event2);
        await context.SaveChangesAsync();

        var request = new GetEventsRequest(
            Title: "Music",
            Date: null,
            Venue: null,
            CategoryId: null,
            Page: 1
        );

        var result = await service.GetFilteredEventsAsync(request);

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal("Music Festival", result.Items.First().Title);
    }

    [Fact]
    public async Task GetFilteredEventsAsync_ShouldReturnFilteredEventsByDate()
    {
        var context = CreateInMemoryContext();
        var unitOfWork = new UnitOfWork(context);
        var service = new EventService(unitOfWork, new HttpClient());

        var event1 = new Event
        {
            Id = Guid.NewGuid(),
            Title = "Music Festival",
            Venue = "Arena",
            StartDate = new DateOnly(2024, 1, 1),
            Category = new EventCategory { Id = Guid.NewGuid(), Name = "Test Category" }
        };
        var event2 = new Event
        {
            Id = Guid.NewGuid(),
            Title = "Tech Conference",
            Venue = "Convention Center",
            StartDate = new DateOnly(2024, 2, 1),
            Category = new EventCategory { Id = Guid.NewGuid(), Name = "Test Category" }
        };

        await context.Events.AddRangeAsync(event1, event2);
        await context.SaveChangesAsync();

        var request = new GetEventsRequest(
            Title: null,
            Date: new DateOnly(2024, 1, 1),
            Venue: null,
            CategoryId: null,
            Page: 1
        );

        var result = await service.GetFilteredEventsAsync(request);

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal("Music Festival", result.Items.First().Title);
    }

    [Fact]
    public async Task GetFilteredEventsAsync_ShouldReturnEmpty_WhenNoMatches()
    {
        var context = CreateInMemoryContext();
        var unitOfWork = new UnitOfWork(context);
        var service = new EventService(unitOfWork, new HttpClient());

        var event1 = new Event
        {
            Id = Guid.NewGuid(),
            Title = "Music Festival",
            Venue = "Arena",
            StartDate = new DateOnly(2024, 1, 1),
            Category = new EventCategory { Id = Guid.NewGuid(), Name = "Test Category" }
        };
        var event2 = new Event
        {
            Id = Guid.NewGuid(),
            Title = "Tech Conference",
            Venue = "Convention Center",
            StartDate = new DateOnly(2024, 2, 1),
            Category = new EventCategory { Id = Guid.NewGuid(), Name = "Test Category" }
        };

        await context.Events.AddRangeAsync(event1, event2);
        await context.SaveChangesAsync();

        var request = new GetEventsRequest(
            Title: "Nonexistent Event",
            Date: null,
            Venue: null,
            CategoryId: null,
            Page: 1
        );

        var result = await service.GetFilteredEventsAsync(request);

        Assert.NotNull(result);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task GetFilteredEventsAsync_ShouldPaginateResults()
    {
        var context = CreateInMemoryContext();
        var unitOfWork = new UnitOfWork(context);
        var service = new EventService(unitOfWork, new HttpClient());

        for (int i = 0; i < 15; i++)
        {
            var newEvent = new Event
            {
                Id = Guid.NewGuid(),
                Title = $"Event {i + 1}",
                Venue = "Test Venue",
                StartDate = new DateOnly(2024, 1, 1),
                Category = new EventCategory { Id = Guid.NewGuid(), Name = "Test Category" }
            };
            await context.Events.AddAsync(newEvent);
        }

        await context.SaveChangesAsync();

        var request = new GetEventsRequest(
            Title: null,
            Date: null,
            Venue: null,
            CategoryId: null,
            Page: 2
        );

        var result = await service.GetFilteredEventsAsync(request);

        Assert.NotNull(result);
        Assert.Equal(7, result.Items.Count());
    }

    [Fact]
    public async Task AddEventAsync_ShouldAddEvent()
    {
        var context = CreateInMemoryContext();
        var unitOfWork = new UnitOfWork(context);
        var httpClient = new HttpClient();
        var service = new EventService(unitOfWork, httpClient);

        var category = new EventCategory { Id = Guid.NewGuid(), Name = "Test Category" };
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        var request = new CreateEventRequest(
            "Test Event",
            "Description",
            DateOnly.FromDateTime(DateTime.UtcNow),
            TimeOnly.FromDateTime(DateTime.UtcNow),
            "Venue",
            0.0,
            0.0,
            category,
            100,
            null
        );

        await service.AddEventAsync(request);

        var events = await context.Events.ToListAsync();
        Assert.Single(events);
        Assert.Equal("Test Event", events[0].Title);
    }

    [Fact]
    public async Task AddEventAsync_ShouldThrowIfCategoryNotFound()
    {
        var context = CreateInMemoryContext();
        var unitOfWork = new UnitOfWork(context);
        var httpClient = new HttpClient();
        var service = new EventService(unitOfWork, httpClient);

        var category = new EventCategory { Id = Guid.NewGuid(), Name = "Invalid Category" };

        var request = new CreateEventRequest(
            "Test Event",
            "Description",
            DateOnly.FromDateTime(DateTime.UtcNow),
            TimeOnly.FromDateTime(DateTime.UtcNow),
            "Venue",
            0.0,
            0.0,
            category,
            100,
            null
        );

        await Assert.ThrowsAsync<ArgumentException>(() => service.AddEventAsync(request));
    }

    [Fact]
    public async Task UpdateEventAsync_ShouldUpdateEvent()
    {
        var context = CreateInMemoryContext();
        var unitOfWork = new UnitOfWork(context);
        var httpClient = new HttpClient();
        var service = new EventService(unitOfWork, httpClient);

        var category = new EventCategory { Id = Guid.NewGuid(), Name = "Test Category" };
        await context.Categories.AddAsync(category);

        var eventToUpdate = new Event
        {
            Id = Guid.NewGuid(),
            Title = "Original Title",
            Category = category
        };
        await context.Events.AddAsync(eventToUpdate);
        await context.SaveChangesAsync();

        var request = new UpdateEventRequest(
            eventToUpdate.Id,
            "Updated Title",
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );

        await service.UpdateEventAsync(request);

        var updatedEvent = await context.Events.FirstOrDefaultAsync(e => e.Id == eventToUpdate.Id);
        Assert.NotNull(updatedEvent);
        Assert.Equal("Updated Title", updatedEvent.Title);
    }

    [Fact]
    public async Task UpdateEventAsync_ShouldThrowIfEventNotFound()
    {
        var context = CreateInMemoryContext();
        var unitOfWork = new UnitOfWork(context);
        var httpClient = new HttpClient();
        var service = new EventService(unitOfWork, httpClient);

        var request = new UpdateEventRequest(
            Guid.NewGuid(),
            "Updated Title",
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );

        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateEventAsync(request));
    }

    [Fact]
    public async Task DeleteEventAsync_ShouldDeleteEventAndRegistrations()
    {
        var context = CreateInMemoryContext();
        var unitOfWork = new UnitOfWork(context);
        var httpClient = new HttpClient();
        var service = new EventService(unitOfWork, httpClient);

        var category = new EventCategory { Id = Guid.NewGuid(), Name = "Test Category" };
        await context.Categories.AddAsync(category);

        var eventToDelete = new Event
        {
            Id = Guid.NewGuid(),
            Title = "Event to Delete",
            Category = category,
            Registrations = new List<EventRegistration>
        {
            new EventRegistration
            {
                Id = Guid.NewGuid(),
                Name = "User1",
                Surname = "Surname1",
                Email = "user1@example.com",
                DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-30)),
                EventId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                RegistrationDate = DateTime.UtcNow
            },
            new EventRegistration
            {
                Id = Guid.NewGuid(),
                Name = "User2",
                Surname = "Surname2",
                Email = "user2@example.com",
                DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
                EventId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                RegistrationDate = DateTime.UtcNow
            }
        }
        };

        await context.Events.AddAsync(eventToDelete);
        await context.SaveChangesAsync();

        var result = await service.DeleteEventAsync(eventToDelete.Id);

        Assert.True(result);
        Assert.Empty(context.Events);
        Assert.Empty(context.Registrations);
    }


    [Fact]
    public async Task DeleteEventAsync_ShouldReturnFalseIfEventNotFound()
    {
        var context = CreateInMemoryContext();
        var unitOfWork = new UnitOfWork(context);
        var httpClient = new HttpClient();
        var service = new EventService(unitOfWork, httpClient);

        var result = await service.DeleteEventAsync(Guid.NewGuid());

        Assert.False(result);
    }

    [Fact]
    public async Task GetUsersEventsAsync_ShouldReturnUserEvents()
    {
        var context = CreateInMemoryContext();
        var unitOfWork = new UnitOfWork(context);
        var httpClient = new HttpClient();
        var service = new EventService(unitOfWork, httpClient);

        var hasher = new PasswordHasher();

        var user = User.Create(Guid.NewGuid(), "TestUser", hasher.GenerateHash("123321"), "test@mail.com");
        var category = new EventCategory { Id = Guid.NewGuid(), Name = "Test Category" };

        var event1 = new Event { Id = Guid.NewGuid(), Title = "Event 1", Category = category };
        var event2 = new Event { Id = Guid.NewGuid(), Title = "Event 2", Category = category };

        await context.Users.AddAsync(user);
        await context.Categories.AddAsync(category);
        await context.Events.AddRangeAsync(event1, event2);

        var registration1 = new EventRegistration
        {
            Id = Guid.NewGuid(),
            EventId = event1.Id,
            UserId = user.Id,
            Name = "Test User",
            Surname = "Test Surname",
            Email = "test@mail.com",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
            RegistrationDate = DateTime.UtcNow
        };

        var registration2 = new EventRegistration
        {
            Id = Guid.NewGuid(),
            EventId = event2.Id,
            UserId = user.Id,
            Name = "Test User",
            Surname = "Test Surname",
            Email = "test@mail.com",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
            RegistrationDate = DateTime.UtcNow
        };

        await context.Registrations.AddRangeAsync(registration1, registration2);

        await context.SaveChangesAsync();

        var request = new UsersEventsRequest(user.Id, 1);

        var result = await service.GetUsersEventsAsync(request);

        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count());
    }


    [Fact]
    public async Task GetUsersEventsAsync_ShouldThrowIfUserNotFound()
    {
        var context = CreateInMemoryContext();
        var unitOfWork = new UnitOfWork(context);
        var httpClient = new HttpClient();
        var service = new EventService(unitOfWork, httpClient);

        var request = new UsersEventsRequest(Guid.NewGuid(), 1);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetUsersEventsAsync(request));
    }
}
