﻿using Eventer.Application.Interfaces.Auth;
using Eventer.Domain.Models;
using Eventer.Infrastructure.Data;

namespace Eventer.Infrastructure
{
    public static class DbInitializer
    {
        public static void Initialize(EventerDbContext context, IPasswordHasher hasher)
        {
            context.Database.EnsureCreated();

            var users = context.Users.ToList();  
            context.Users.RemoveRange(users);   
            context.SaveChanges();

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

            if(!context.Events.Any())
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
                        Latitude = 55.7558,
                        Longitude = 37.6173,
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
                        Latitude = 55.7558,
                        Longitude = 37.6173,
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
                        Latitude = 55.7558,
                        Longitude = 37.6173,
                        MaxParticipants = 75,
                        Category = firstCategory,
                        Registrations = new List<EventRegistration>(),
                        ImageURLs = null
                    }
                };

                context.Events.AddRange(testEvents);

                context.SaveChanges();
            }

            if(!context.Users.Any())
            {
                var testUser = User.Create(Guid.NewGuid(), "TestUser", hasher.GenerateHash("123qwe"), "test@mail.com");

                var testAdmin = User.Create(Guid.NewGuid(), "TestAdmin", hasher.GenerateHash("123qwe"), "test_admin@mail.com");
                testAdmin.Role = UserRole.Admin;

                context.Users.AddAsync(testUser);
                context.Users.AddAsync(testAdmin);

                context.SaveChanges();
            }

        }
    }
}