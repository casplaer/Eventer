using AutoMapper;
using Eventer.Application.Interfaces.Services;
using Eventer.Application.Mappings.Events;
using Eventer.Application.UseCases.Events;
using Eventer.Domain.Contracts.Events;
using Eventer.Domain.Models;
using Eventer.Infrastructure.Data;
using Eventer.Infrastructure.Repositories;
using Eventer.Infrastructure.Validators;
using Eventer.Tests;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

public class EventUseCasesTests
{
    private UnitOfWork CreateUnitOfWork(EventerDbContext context)
    {
        return new UnitOfWork(
            context,
            new EventRepository(context),
            new CategoryRepository(context),
            new UserRepository(context),
            new Repository<EventRegistration>(context)
        );
    }

    private EventerDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<EventerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new EventerDbContext(options);
    }

    [Fact]
    public async Task GetFilteredEventsUseCase_ShouldReturnFilteredEventsByTitle()
    {
        var context = CreateInMemoryContext();

        var unitOfWork = CreateUnitOfWork(context);

        var useCase = new GetFilteredEventsUseCase(unitOfWork);

        var category = TestDataGenerator.GenerateCategory(name: "Test Category");
        var testEvents = TestDataGenerator.GenerateTestEvents(category);

        await context.Events.AddRangeAsync(testEvents);
        await context.SaveChangesAsync();

        var request = new GetEventsRequest(
            Title: "Music",
            Date: null,
            Venue: null,
            CategoryId: null,
            Page: 1
        );

        var result = await useCase.ExecuteAsync(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count());
        Assert.Equal("Music Festival", result.Items.First().Title);
    }

    [Fact]
    public async Task GetFilteredEventsUseCase_ShouldReturnFilteredEventsByDate()
    {
        var context = CreateInMemoryContext();
        var unitOfWork = CreateUnitOfWork(context);

        var useCase = new GetFilteredEventsUseCase(unitOfWork);

        var category = TestDataGenerator.GenerateCategory(name: "Test Category");
        var testEvents = TestDataGenerator.GenerateTestEvents(category);

        testEvents[0].StartDate = new DateOnly(2024, 1, 1); 
        testEvents[1].StartDate = new DateOnly(2024, 2, 1); 

        await context.Events.AddRangeAsync(testEvents);
        await context.SaveChangesAsync();

        var request = new GetEventsRequest(
            Title: null,
            Date: new DateOnly(2024, 1, 1),
            Venue: null,
            CategoryId: null,
            Page: 1
        );

        var result = await useCase.ExecuteAsync(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal("Music Festival", result.Items.First().Title);
    }

    [Fact]
    public async Task GetFilteredEventsUseCase_ShouldReturnEmpty_WhenNoMatches()
    {
        var context = CreateInMemoryContext();
        var unitOfWork = CreateUnitOfWork(context);

        var useCase = new GetFilteredEventsUseCase(unitOfWork);

        var category = TestDataGenerator.GenerateCategory(name: "Test Category");
        var testEvents = TestDataGenerator.GenerateTestEvents(category);

        await context.Events.AddRangeAsync(testEvents);
        await context.SaveChangesAsync();

        var request = new GetEventsRequest(
            Title: "Nonexistent Event",
            Date: null,
            Venue: null,
            CategoryId: null,
            Page: 1
        );

        var result = await useCase.ExecuteAsync(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result.Items);
    }



    [Fact]
    public async Task GetFilteredEventsUseCase_ShouldPaginateResults()
    {
        var context = CreateInMemoryContext();

        var unitOfWork = CreateUnitOfWork(context);

        var useCase = new GetFilteredEventsUseCase(unitOfWork);

        var category = TestDataGenerator.GenerateCategory(name: "Test Category");
        var testEvents = TestDataGenerator.GenerateTestEvents(category);

        await context.Events.AddRangeAsync(testEvents);
        await context.SaveChangesAsync();

        var request = new GetEventsRequest(
            Title: null,
            Date: null,
            Venue: null,
            CategoryId: null,
            Page: 2
        );

        var result = await useCase.ExecuteAsync(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(7, result.Items.Count());
        Assert.Equal("Science Symposium", result.Items.First().Title);
    }

    [Fact]
    public async Task AddEventUseCase_ShouldAddEvent()
    {
        var context = CreateInMemoryContext();

        var unitOfWork = CreateUnitOfWork(context);

        var imageService = Substitute.For<IImageService>();
        var validator = Substitute.For<IValidator<Event>>();
        var httpClient = new HttpClient();
        var mapper = Substitute.For<IMapper>();

        validator.ValidateAsync(Arg.Any<Event>(), Arg.Any<CancellationToken>())
             .Returns(new ValidationResult());

        var useCase = new AddEventUseCase(unitOfWork, httpClient, imageService, validator, mapper);

        var category = TestDataGenerator.GenerateCategory(name: "Test Category");

        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        var request = new CreateEventRequest(
            Title: "Test Event",
            Description: "Description",
            StartDate: DateOnly.FromDateTime(DateTime.UtcNow),
            StartTime: TimeOnly.FromDateTime(DateTime.UtcNow),
            Venue: "Venue",
            Category: category,
            MaxParticipants: 100,
            Images: null
        );

        await useCase.ExecuteAsync(request, CancellationToken.None);

        var events = await context.Events.ToListAsync();
        Assert.Single(events);
        Assert.Equal("Test Event", events[0].Title);
        Assert.Equal("Venue", events[0].Venue);
    }

    [Fact]
    public async Task AddEventUseCase_ShouldThrowIfCategoryNotFound()
    {
        var context = CreateInMemoryContext();

        var unitOfWork = CreateUnitOfWork(context);

        var imageService = Substitute.For<IImageService>();
        var validator = Substitute.For<IValidator<Event>>();
        var httpClient = new HttpClient();

        validator.ValidateAsync(Arg.Any<Event>(), Arg.Any<CancellationToken>())
             .Returns(new ValidationResult());

        var mapper = Substitute.For<IMapper>();

        var useCase = new AddEventUseCase(unitOfWork, httpClient, imageService, validator, mapper);

        var invalidCategory = TestDataGenerator.GenerateCategory(name: "Invalid Category");

        var request = new CreateEventRequest(
            Title: "Test Event",
            Description: "Description",
            StartDate: DateOnly.FromDateTime(DateTime.UtcNow),
            StartTime: TimeOnly.FromDateTime(DateTime.UtcNow),
            Venue: "Venue",
            Category: invalidCategory,
            MaxParticipants: 100,
            Images: null
        );

        await Assert.ThrowsAsync<KeyNotFoundException>(() => useCase.ExecuteAsync(request, CancellationToken.None));
    }


    [Fact]
    public async Task UpdateEventUseCase_ShouldUpdateEvent()
    {
        var context = CreateInMemoryContext();

        var unitOfWork = CreateUnitOfWork(context);

        var httpClient = new HttpClient();
        var imageService = Substitute.For<IImageService>();
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<UpdateEventProfile>()));
        var validator = Substitute.For<IValidator<Event>>();

        validator.ValidateAsync(Arg.Any<Event>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        var useCase = new UpdateEventUseCase(unitOfWork, httpClient, mapper, imageService, validator);

        var category = TestDataGenerator.GenerateCategory(name: "Test Category");
        await context.Categories.AddAsync(category);

        var eventToUpdate = TestDataGenerator.GenerateEvent(
            id: Guid.NewGuid(),
            title: "Original Title",
            category: category
        );
        await context.Events.AddAsync(eventToUpdate);
        await context.SaveChangesAsync();

        var request = new UpdateEventRequest(
            Id: eventToUpdate.Id,
            Title: "Updated Title",
            Description: null,
            StartDate: null,
            StartTime: null,
            Venue: null,
            Latitude: null,
            Longitude: null,
            Category: null,
            MaxParticipants: null,
            Images: null,
            ExistingImages: null,
            RemovedImages: null
        );

        await useCase.ExecuteAsync(request, CancellationToken.None);

        var updatedEvent = await context.Events.FirstOrDefaultAsync(e => e.Id == eventToUpdate.Id);
        Assert.NotNull(updatedEvent);
        Assert.Equal("Updated Title", updatedEvent.Title);
    }



    [Fact]
    public async Task UpdateEventUseCase_ShouldThrowIfEventNotFound()
    {
        var context = CreateInMemoryContext();

        var unitOfWork = CreateUnitOfWork(context);

        var httpClient = new HttpClient();
        var imageService = Substitute.For<IImageService>();
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<UpdateEventProfile>()));
        var validator = Substitute.For<IValidator<Event>>();

        var useCase = new UpdateEventUseCase(unitOfWork, httpClient, mapper, imageService, validator);

        var request = new UpdateEventRequest(
            Id: Guid.NewGuid(),
            Title: "Updated Title",
            Description: null,
            StartDate: null,
            StartTime: null,
            Venue: null,
            Latitude: null,
            Longitude: null,
            Category: null,
            MaxParticipants: null,
            Images: null,
            ExistingImages: null,
            RemovedImages: null
        );

        await Assert.ThrowsAsync<KeyNotFoundException>(() => useCase.ExecuteAsync(request, CancellationToken.None));
    }


    [Fact]
    public async Task DeleteEventUseCase_ShouldDeleteEventAndRegistrations()
    {
        var context = CreateInMemoryContext();

        var unitOfWork = CreateUnitOfWork(context);

        var imageService = Substitute.For<IImageService>();
        var useCase = new DeleteEventUseCase(unitOfWork, imageService);

        var category = TestDataGenerator.GenerateCategory(name: "Test Category");

        var registrations = new List<EventRegistration>
        {
            TestDataGenerator.GenerateRegistration(),
            TestDataGenerator.GenerateRegistration()
        };

        var eventToDelete = TestDataGenerator.GenerateEvent(
            id: Guid.NewGuid(),
            title: "Event to Delete",
            category: category,
            registrations: registrations
        );

        await context.Categories.AddAsync(category);
        await context.Events.AddAsync(eventToDelete);
        await context.SaveChangesAsync();

        var result = await useCase.ExecuteAsync(eventToDelete.Id, CancellationToken.None);

        Assert.True(result);
        Assert.Empty(context.Events);
        Assert.Empty(context.Registrations);
        imageService.Received(1).DeleteImages(eventToDelete.ImageURLs, Arg.Any<string>());
    }



    [Fact]
    public async Task DeleteEventUseCase_ShouldReturnFalseIfEventNotFound()
    {
        var context = CreateInMemoryContext();

        var unitOfWork = CreateUnitOfWork(context);

        var imageService = Substitute.For<IImageService>();
        var useCase = new DeleteEventUseCase(unitOfWork, imageService);

        var nonExistentEventId = Guid.NewGuid();

        var result = await useCase.ExecuteAsync(nonExistentEventId, CancellationToken.None);

        Assert.False(result);
        imageService.DidNotReceiveWithAnyArgs().DeleteImages(default, default);
    }


    [Fact]
    public async Task GetUsersEventsUseCase_ShouldReturnUserEvents()
    {
        var context = CreateInMemoryContext();

        var unitOfWork = CreateUnitOfWork(context);

        var useCase = new GetUsersEventsUseCase(unitOfWork);

        var user = TestDataGenerator.GenerateUser();
        var category = TestDataGenerator.GenerateCategory(name: "Test Category");

        var testEvents = new List<Event>
        {
            TestDataGenerator.GenerateEvent(category: category),
            TestDataGenerator.GenerateEvent(category: category)
        };

        var registrations = new List<EventRegistration>
        {
            TestDataGenerator.GenerateRegistration(eventId: testEvents[0].Id, userId: user.Id),
            TestDataGenerator.GenerateRegistration(eventId: testEvents[1].Id, userId: user.Id)
        };

        await context.Users.AddAsync(user);
        await context.Categories.AddAsync(category);
        await context.Events.AddRangeAsync(testEvents);
        await context.Registrations.AddRangeAsync(registrations);
        await context.SaveChangesAsync();

        var request = new UsersEventsRequest(user.Id, 1);

        var result = await useCase.ExecuteAsync(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count());
    }



    [Fact]
    public async Task GetUsersEventsUseCase_ShouldThrowIfUserNotFound()
    {
        var context = CreateInMemoryContext();

        var unitOfWork = CreateUnitOfWork(context);

        var useCase = new GetUsersEventsUseCase(unitOfWork);

        var nonExistentUserId = Guid.NewGuid();
        var request = new UsersEventsRequest(nonExistentUserId, 1);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => useCase.ExecuteAsync(request, CancellationToken.None));
    }

}
