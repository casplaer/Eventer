using Eventer.Contracts.DTOs.Auth;

namespace Eventer.Domain.Contracts.Auth
{
    public record LoginResponse(
        string AccessToken,
        string RefreshToken,
        UserDTO User);
}
