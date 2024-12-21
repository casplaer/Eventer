using AutoMapper;
using Eventer.Domain.Contracts.Categories;
using Eventer.Domain.Models;

namespace Eventer.Application.Mappings.Categories
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
