using Eventer.Application.Contracts.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventer.Application.Interfaces.UseCases.Auth
{
    public interface IRegisterUserUseCase
    {
        Task Execute(RegisterUserRequest request, CancellationToken cancellationToken);
    }
}
