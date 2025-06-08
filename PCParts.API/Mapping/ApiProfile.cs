using AutoMapper;
using PCParts.Domain.Entities;

namespace PCParts.API.Mapping;

public class ApiProfile : Profile
{
    public ApiProfile()
    {
        CreateMap<Category, Application.Model.Models.Category>().ReverseMap();
        CreateMap<Component, Application.Model.Models.Component>().ReverseMap();
        CreateMap<Specification, Application.Model.Models.Specification>().ReverseMap();

        CreateMap<SpecificationValue, Application.Model.Models.SpecificationValue>().ReverseMap()
            .ForMember(x => x.Id, x => x.MapFrom(x => x.Id))
            .ForMember(x => x.Value, x => x.MapFrom(x => x.Value));

        CreateMap<Application.Model.Models.SpecificationValue, SpecificationValue>();
        CreateMap<Application.Model.Models.Component, Component>();

        CreateMap<Application.Model.Models.PendingUser, PendingUser>().ReverseMap();
    }
}