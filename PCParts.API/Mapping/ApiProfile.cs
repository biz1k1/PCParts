using AutoMapper;
using PCParts.API.Model;
using Category = PCParts.Application.Model.Models.Category;
using Component = PCParts.Application.Model.Models.Component;
using Specification = PCParts.Application.Model.Models.Specification;

namespace PCParts.API.Mapping;

public class ApiProfile : Profile
{
    public ApiProfile()
    {
        CreateMap<Category, Model.Category>();
        CreateMap<Component, Model.Component>().ReverseMap();
        CreateMap<Specification, Model.Specification>().ReverseMap();
    }
}