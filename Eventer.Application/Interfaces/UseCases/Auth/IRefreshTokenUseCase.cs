namespace Eventer.Application.Interfaces.UseCases.Auth
{
    public interface IRefreshTokenUseCase
    {
        Task<string> Execute(string refreshToken, CancellationToken cancellationToken);
    }
}
