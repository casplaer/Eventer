using Eventer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventer.Application.Contracts.Auth
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
