using AutoMapper;
using Eventer.Contracts.Requests.Enrollments;
using Eventer.Domain.Models;

namespace Eventer.Application.Mappings.Enrollments
{
    public class UpdateEnrollmentProfile : Profile
    {
        public UpdateEnrollmentProfile()
        {
            CreateMap<UpdateEnrollRequest, EventRegistration>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Name)))
                .ForMember(dest => dest.Surname, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Surname)))
                .ForMember(dest => dest.Email, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Email)))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));
        }
    }
}
