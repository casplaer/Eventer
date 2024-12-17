namespace Eventer.Application.Contracts.Auth
{
    public record LoginResponse(
        string AccessToken,
        string RefreshToken,
        UserDTO User);
}
