using Eventer.Application.Interfaces.UseCases.Events;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;

namespace Eventer.Application.UseCases.Events
{
    public class GetAllEventsUseCase : IGetAllEventsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllEventsUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Event>> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await _unitOfWork.Events.GetAllAsync(cancellationToken);
        }
    }
}
