using System.ComponentModel.DataAnnotations;

namespace Eventer.Domain.Contracts.Auth
{
    public record RegisterUserRequest(
        [Required] string UserName,
        [Required] string Email,
        [Required] string Password,
        [Required] string PasswordConfirm);
}
