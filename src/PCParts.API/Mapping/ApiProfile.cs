using AutoMapper;
using PCParts.API.Model.Models;

namespace PCParts.API.Mapping;

public class ApiProfile : Profile
{
    public ApiProfile()
    {
        CreateMap<Category, Application.Model.Models.Category>().ReverseMap();
        CreateMap<Component, Application.Model.Models.Component>().ReverseMap();
        CreateMap<Specification, Application.Model.Models.Specification>().ReverseMap();
        CreateMap<Application.Model.Models.Specification, Specification>().ReverseMap();

        CreateMap<SpecificationValue, Application.Model.Models.SpecificationValue>().ReverseMap()
            .ForMember(x => x.Id, x => x.MapFrom(x => x.Id))
            .ForMember(x => x.Value, x => x.MapFrom(x => x.Value));

        CreateMap<PendingUser, Model.Models.PendingUser>().ReverseMap();

        CreateMap<Application.Model.Models.Category, Model.Models.Category>();
        CreateMap<Application.Model.Models.Component, Model.Models.Component>()
            .ForMember(x => x.Category, x => x.MapFrom(x => x.Category.Name)); 
        CreateMap<Application.Model.Models.SpecificationValue, Model.Models.SpecificationValue>();
    }
}
