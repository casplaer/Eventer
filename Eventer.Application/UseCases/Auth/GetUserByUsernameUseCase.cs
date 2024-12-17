using Eventer.Application.Interfaces.UseCases.Auth;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;

namespace Eventer.Application.UseCases.Auth
{
    public class GetUserByUsernameUseCase : IGetUserByUsernameUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByUsernameUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Execute(string username, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByUserNameAsync(username, cancellationToken);

            return user;
        }
    }

}
