using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventer.Application.Contracts.Auth
{
    public record TokensResponse(
        string AccessToken,
        string RefreshToken);
}
