using AutoMapper;
using Eventer.Contracts.DTOs.Events;
using Eventer.Domain.Models;

namespace Eventer.Application.Mappings.Events
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
