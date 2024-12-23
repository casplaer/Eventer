using AutoMapper;
using Eventer.Contracts.Responses.Enrollments;
using Eventer.Domain.Models;

namespace Eventer.Application.Mappings.Enrollments
{
    public class SingleEnrollmentResponseProfile : Profile
    {
        public SingleEnrollmentResponseProfile()
        {
            CreateMap<EventRegistration, SingleEnrollmentResponse>()
                .ForMember(dest => dest.EnrollmentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.EventId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));
        }
    }
}
