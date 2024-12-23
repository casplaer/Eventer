﻿using Eventer.Contracts.Requests.Auth;

namespace Eventer.Application.Interfaces.UseCases.Auth
{
    public interface IRegisterUserUseCase
    {
        Task Execute(RegisterUserRequest request, CancellationToken cancellationToken);
    }
}
