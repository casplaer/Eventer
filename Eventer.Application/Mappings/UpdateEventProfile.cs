using AutoMapper;
using Eventer.Domain.Contracts.Events;
using Eventer.Domain.Models;

namespace Eventer.Application.Mappings
{
    public class UpdateEventProfile : Profile
    {
        public UpdateEventProfile()
        {
            CreateMap<UpdateEventRequest, Event>()
                .ForMember(dest => dest.Title, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Title)))
                .ForMember(dest => dest.Description, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Description)))
                .ForMember(dest => dest.StartDate, opt => opt.Condition(src => src.StartDate.HasValue))
                .ForMember(dest => dest.StartTime, opt => opt.Condition(src => src.StartTime.HasValue))
                .ForMember(dest => dest.Venue, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Venue)))
                .ForMember(dest => dest.MaxParticipants, opt => opt.Condition(src => src.MaxParticipants.HasValue))
                .ForMember(dest => dest.ImageURLs, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore());
        }
    }
}
