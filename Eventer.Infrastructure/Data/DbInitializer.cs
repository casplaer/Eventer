using Eventer.Application.Interfaces.Auth;
using Eventer.Domain.Enums;
using Eventer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Eventer.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void SeedDatabase(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var scopedServices = scope.ServiceProvider;

            try
            {
                var context = scopedServices.GetRequiredService<EventerDbContext>();
                var hasher = scopedServices.GetRequiredService<IPasswordHasher>();
                context.Database.Migrate();
                Initialize(context, hasher);
            }
            catch (Exception ex)
            {
                var logger = scopedServices.GetRequiredService<ILogger<EventerDbContext>>();
                logger.LogError(ex, "An error occurred while migrating or seeding the database.");
            }
        }

        public static void Initialize(EventerDbContext context, IPasswordHasher hasher)
        {
            context.Database.EnsureCreated();

            if (context.Users.FirstOrDefault(u => u.Role == UserRole.Admin) == null)
            {
                var adminUser = User.Create
                (
                    id: Guid.NewGuid(),
                    userName: "TestAdmin",
                    email: "Admin@mail.com",
                    passwordHash: hasher.GenerateHash("123qwe")
                );
                adminUser.Role = UserRole.Admin;
                adminUser.NormalizedEmail = "admin@mail.com";
                context.Users.AddAsync(adminUser);
            }

            if (context.Users.FirstOrDefault(u => u.Role == UserRole.User) == null)
            {
                context.Users.AddAsync(User.Create
                (
                    id: Guid.NewGuid(),
                    userName: "TestUser",
                    email: "user@mail.com",
                    passwordHash: hasher.GenerateHash("123qwe")
                ));
            }

            if (!context.Categories.Any())
            {
                var testCategories = new List<EventCategory>
                {
                    new EventCategory
                    {
                        Name = "Тестовая категория",
                        Description = "Тестовое описание"
                    },

                    new EventCategory
                    {
                        Name = "Test category",
                        Description = "Test description"
                    },
                };

                context.Categories.AddRange(testCategories);
                context.SaveChanges();
            }

            var firstCategory = context.Categories.FirstOrDefault();

            if (!context.Events.Any())
            {
                var testEvents = new List<Event>
                {
                    new Event
                    {
                        Id = Guid.NewGuid(),
                        Title = "Тестовое событие1",
                        Description = "Описание тестового события 1",
                        StartDate = new DateOnly(2024, 11, 20),
                        StartTime = new TimeOnly(12, 0),
                        Venue = "Место проведения 1",
                        MaxParticipants = 100,
                        Category = firstCategory,
                        Registrations = new List<EventRegistration>(),
                        ImageURLs = null
                    },
                    new Event
                    {
                        Id = Guid.NewGuid(),
                        Title = "Тестовое событие2",
                        Description = "Описание тестового события 2",
                        StartDate = new DateOnly(2024, 11, 21),
                        StartTime = new TimeOnly(14, 0),
                        Venue = "Место проведения 2",
                        MaxParticipants = 50,
                        Category = firstCategory,
                        Registrations = new List<EventRegistration>(),
                        ImageURLs = null
                    },
                    new Event
                    {
                        Id = Guid.NewGuid(),
                        Title = "Тестовое событие3",
                        Description = "Описание тестового события 3",
                        StartDate = new DateOnly(2024, 11, 22),
                        StartTime = new TimeOnly(16, 0),
                        Venue = "Место проведения 3",
                        MaxParticipants = 75,
                        Category = firstCategory,
                        Registrations = new List<EventRegistration>(),
                        ImageURLs = null
                    }
                };

                context.Events.AddRange(testEvents);

                context.SaveChanges();
            }
        }
    }
}
