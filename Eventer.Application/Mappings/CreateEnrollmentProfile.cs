using AutoMapper;
using Eventer.Application.Contracts.Enrollments;
using Eventer.Domain.Models;

namespace Eventer.Application.Mappings
{
    public class CreateEnrollmentProfile : Profile
    {
        public CreateEnrollmentProfile()
        {
            CreateMap<EnrollRequest, EventRegistration>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.SurName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.RegistrationDate, opt => opt.Ignore()) 
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) 
                .ForMember(dest => dest.EventId, opt => opt.Ignore()); 
        }
    }
}
