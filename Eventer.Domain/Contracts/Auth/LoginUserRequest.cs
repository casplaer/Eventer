using System.ComponentModel.DataAnnotations;

namespace Eventer.Application.Contracts.Auth
{
    public record LoginUserRequest(
        [Required]string UserName, 
        [Required]string Password);
}
