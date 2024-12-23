using Eventer.Application.Interfaces.UseCases.Events;
using Eventer.Contracts.Requests.Events;
using Eventer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Eventer.Domain.Contracts;
using Eventer.Domain.Interfaces.Repositories;

namespace Eventer.Application.UseCases.Events
{
    public class GetFilteredEventsUseCase : IGetFilteredEventsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int pageSize = 10;

        public GetFilteredEventsUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<Event>> ExecuteAsync(GetEventsRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            }

            return await _unitOfWork.Events.GetFilteredEventsAsync(
                request.Title,
                request.Date,
                request.Venue,
                request.CategoryId,
                request.Page, 
                cancellationToken);
        }
    }

}
