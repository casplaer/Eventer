using AutoMapper;
using Eventer.Domain.Contracts.Events;
using Eventer.Domain.Models;

namespace Eventer.Application.Mappings
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<Event, EventDTO>()
                .ForCtorParam("CurrentRegistrations", opt => opt.MapFrom(src => src.Registrations.Count));
        }
    }
}
