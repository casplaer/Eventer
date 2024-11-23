using Eventer.Domain.Models;

namespace Eventer.Application.Interfaces.Auth
{
    public interface IJwtProvider
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}
