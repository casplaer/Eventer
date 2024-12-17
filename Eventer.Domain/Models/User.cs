using Eventer.Domain.Enums;
using Eventer.Domain.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Eventer.Domain.Models
{
    public class User
    {
        private User(Guid id, string userName, string passwordHash, string email)
        {
            Id = id;
            UserName = userName;
            PasswordHash = passwordHash;
            Email = email;
            NormalizedEmail = EmailNormalizer.Normalize(email);
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Имя пользователя должно быть в пределах от 5 до 50 символов.")]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Неправильный формат электронной почты.")]
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string PasswordHash { get; set; }

        public ICollection<EventRegistration> EventRegistrations { get; set; }

        public UserRole Role { get; set; } = UserRole.User;

        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; } = DateTime.UtcNow.AddDays(7);

        public static User Create(Guid id, string userName, string passwordHash, string email)
        {
            return new User(id, userName, passwordHash, email);
        }
    }
}
