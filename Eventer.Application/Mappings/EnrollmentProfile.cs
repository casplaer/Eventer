﻿using AutoMapper;
using Eventer.Application.Contracts.Enrollments;
using Eventer.Domain.Models;

namespace Eventer.Application.Mappings
{
    public class EnrollmentProfile : Profile
    {
        public EnrollmentProfile()
        {
            CreateMap<UpdateEnrollRequest, EventRegistration>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Name)))
                .ForMember(dest => dest.Surname, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Surname)))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));
        }
    }
}