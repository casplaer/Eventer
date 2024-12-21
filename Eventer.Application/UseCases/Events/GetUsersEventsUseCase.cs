using Eventer.Application.Interfaces.UseCases.Events;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;
using Eventer.Domain.Contracts;
using Eventer.Domain.Contracts.Events;

namespace Eventer.Application.UseCases.Events
{
    public class GetUsersEventsUseCase : IGetUsersEventsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int pageSize = 10;

        public GetUsersEventsUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<Event>> ExecuteAsync(UsersEventsRequest request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {request.UserId} not found.");
            }

            var usersEvents = user.EventRegistrations
                .Select(er => er.Event)            
                .Where(e => e != null)                
                .Distinct()
                .ToList();

            if (usersEvents.Count == 0)
            {
                return new PaginatedResult<Event>
                {
                    Items = new List<Event>(),
                    TotalCount = 0,
                    TotalPages = 1
                };
            }

            int totalCount = usersEvents.Count;
            int page = Math.Max(1, request.Page);
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            usersEvents = usersEvents
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResult<Event>
            {
                Items = usersEvents,
                TotalCount = totalCount,
                TotalPages = totalPages > 0 ? totalPages : 1,
            };
        }
    }
}
