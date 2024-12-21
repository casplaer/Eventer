using AutoMapper;
using Eventer.Application.Interfaces.UseCases.Events;
using Eventer.Domain.Contracts.Events;
using Eventer.Domain.Interfaces.Repositories;

namespace Eventer.Application.UseCases.Events
{
    public class GetEventByIdUseCase : IGetEventByIdUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventByIdUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SingleEventResponse?> ExecuteAsync(Guid id, CancellationToken cancellationToken)
        {
            var eventToReturn = await _unitOfWork.Events.GetByIdAsync(id, cancellationToken);

            if (eventToReturn == null)
            {
                throw new KeyNotFoundException($"Event with ID '{id}' not found.");
            }

            return _mapper.Map<SingleEventResponse>(eventToReturn);
        }
    }
}
