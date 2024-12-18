using AutoMapper;
using Eventer.Domain.Contracts.Events;
using Eventer.Domain.Models;

namespace Eventer.Application.Mappings
{
    public class CreateEventProfile : Profile
    {
        public CreateEventProfile()
        {
            CreateMap<CreateEventRequest, Event>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.Venue, opt => opt.MapFrom(src => src.Venue))
                .ForMember(dest => dest.MaxParticipants, opt => opt.MapFrom(src => src.MaxParticipants));
        }
    }
}
