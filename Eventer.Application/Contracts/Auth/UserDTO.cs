using Eventer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventer.Application.Contracts.Auth
{
    public record UserDTO(
        string UserName,
        string Email,
        ICollection<EventRegistration>? Registrations,
        UserRole Role);
}
