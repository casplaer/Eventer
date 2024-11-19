using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventer.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Имя пользователя должно быть в пределах от 5 до 50 символов.")]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Неправильный формат электронной почты.")]
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public ICollection<EventRegistration> EventRegistrations { get; set; }
    }
}
