using System.ComponentModel.DataAnnotations;

namespace Eventer.Contracts.Requests.Auth
{
    public record LoginUserRequest(
        [Required] string UserName,
        [Required] string Password);
}
