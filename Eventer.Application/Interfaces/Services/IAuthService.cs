using Eventer.Application.Contracts.Auth;
using Eventer.Domain.Models;
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
        Task<TokensResponse> LoginUserAsync(LoginUserRequest request);
        Task<TokensResponse> RefreshTokensAsync(string refreshToken);
        Task<User> GetUserByTokenAsync(string refreshToken);
        Task<User> GetUserByUsernameAsync(string username);
    }
}
