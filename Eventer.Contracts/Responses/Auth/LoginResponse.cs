using Eventer.Contracts.DTOs.Auth;

namespace Eventer.Contracts.Responses.Auth
{
    public record LoginResponse(
        string AccessToken,
        string RefreshToken,
        UserDTO User);
}
