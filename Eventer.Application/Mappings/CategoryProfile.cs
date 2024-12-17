using AutoMapper;
using Eventer.Application.Contracts.Categories;
using Eventer.Domain.Models;

namespace Eventer.Application.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<UpdateCategoryRequest, EventCategory>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Name)))
                .ForMember(dest => dest.Description, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Description)));
        }
    }
}
