using Eventer.Domain.Models;

namespace Eventer.Contracts.DTOs.Auth
{
    public record UserDTO
    {
        public string UserName { get; init; }
        public string Email { get; init; }
        public ICollection<EventRegistration>? Registrations { get; init; }

        public UserDTO() { }

        public UserDTO(string userName, string email, ICollection<EventRegistration>? registrations)
        {
            UserName = userName;
            Email = email;
            Registrations = registrations;
        }
    }

}
