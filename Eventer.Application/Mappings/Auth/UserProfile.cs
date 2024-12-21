using AutoMapper;
using Eventer.Contracts.DTOs.Auth;
using Eventer.Domain.Models;

namespace Eventer.Application.Mappings.Auth
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
