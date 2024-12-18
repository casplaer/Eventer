using AutoMapper;
using Eventer.Application.Contracts.Auth;
using Eventer.Domain.Models;

namespace Eventer.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Registrations, opt => opt.MapFrom(src => src.EventRegistrations));
        }
    }
}
