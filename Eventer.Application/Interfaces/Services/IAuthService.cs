using Eventer.Application.Contracts.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventer.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task RegisterUserAsync(RegisterUserRequest request);
        Task<string> LoginUserAsync(LoginUserRequest request);
    }
}
