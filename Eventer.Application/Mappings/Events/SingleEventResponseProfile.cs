using AutoMapper;
using Eventer.Contracts.Responses.Events;
using Eventer.Domain.Models;

namespace Eventer.Application.Mappings.Events
{
    public class SingleEventResponseProfile : Profile
    {
        public SingleEventResponseProfile()
        {
            CreateMap<Event, SingleEventResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.Venue, opt => opt.MapFrom(src => src.Venue))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.MaxParticipants, opt => opt.MapFrom(src => src.MaxParticipants))
                .ForMember(dest => dest.CurrentRegistrations, opt => opt.MapFrom(src => src.Registrations != null ? src.Registrations.Count : 0))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ImageURLs));
        }
    }
}
