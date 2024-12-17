using Eventer.Domain.Models;

namespace Eventer.Tests
{
    public static class TestDataGenerator
    {
        public static List<Event> GenerateTestEvents(EventCategory? category = null)
        {
            category ??= GenerateCategory();

            return new List<Event>
            {
                GenerateEvent(title: "Music Festival", venue: "Central Park", category: category),
                GenerateEvent(title: "Tech Conference", venue: "Convention Center", category: category),
                GenerateEvent(title: "Art Exhibition", venue: "City Gallery", category: category),
                GenerateEvent(title: "Food Fair", venue: "Town Square", category: category),
                GenerateEvent(title: "Charity Marathon", venue: "Riverside Park", category: category),
                GenerateEvent(title: "Book Fair", venue: "Library Hall", category: category),
                GenerateEvent(title: "Startup Pitch Night", venue: "Tech Hub", category: category),
                GenerateEvent(title: "Science Symposium", venue: "University Auditorium", category: category),
                GenerateEvent(title: "Movie Premiere", venue: "Downtown Cinema", category: category),
                GenerateEvent(title: "Theater Play", venue: "Grand Theater", category: category),
                GenerateEvent(title: "Coding Bootcamp", venue: "Tech School", category: category),
                GenerateEvent(title: "Job Fair", venue: "Expo Center", category: category),
                GenerateEvent(title: "Stand-up Comedy Night", venue: "Comedy Club", category: category),
                GenerateEvent(title: "Classical Music Concert", venue: "Opera House", category: category),
                GenerateEvent(title: "Gaming Tournament", venue: "Esports Arena", category: category)
            };
        }

        public static Event GenerateEvent(
            Guid? id = null,
            string title = "Test Event",
            DateOnly? startDate = null,
            TimeOnly? startTime = null,
            EventCategory? category = null,
            List<EventRegistration>? registrations = null,
            string venue = "Test Venue",
            int maxParticipants = 100
        )
        {
            return new Event
            {
                Id = id ?? Guid.NewGuid(),
                Title = title,
                Description = "Test Event Description",
                StartDate = startDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
                StartTime = startTime ?? TimeOnly.FromDateTime(DateTime.UtcNow),
                Venue = venue,
                Category = category ?? GenerateCategory(),
                MaxParticipants = maxParticipants,
                Registrations = registrations ?? [],
                ImageURLs = []
            };
        }

        public static EventCategory GenerateCategory(Guid? id = null, string name = "Test Category")
        {
            return new EventCategory
            {
                Id = id ?? Guid.NewGuid(),
                Name = name,
                Description = "Test Description"
            };
        }

        public static EventRegistration GenerateRegistration(
            Guid? id = null,
            Guid? eventId = null,
            Guid? userId = null,
            string name = "Test User",
            string surname = "Test Surname",
            string email = "test@example.com"
        )
        {
            return new EventRegistration
            {
                Id = id ?? Guid.NewGuid(),
                EventId = eventId ?? Guid.NewGuid(),
                UserId = userId ?? Guid.NewGuid(),
                Name = name,
                Surname = surname,
                Email = email,
                DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
                RegistrationDate = DateTime.UtcNow
            };
        }

        public static User GenerateUser(
            Guid? id = null,
            string userName = "TestUser",
            string email = "test@example.com",
            string passwordHash = "hashed-password"
        )
        {
            return User.Create(
                id ?? Guid.NewGuid(),
                userName,
                passwordHash,
                email
            );
        }
    }
}
