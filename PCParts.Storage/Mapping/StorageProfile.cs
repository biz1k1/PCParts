using AutoMapper;
using PCParts.Domain.Entities;

namespace PCParts.Storage.Mapping;

public class StorageProfile : Profile
{
    public StorageProfile()
    {
        CreateMap<Category, Application.Model.Models.Category>().ReverseMap();
        CreateMap<Component, Application.Model.Models.Component>()
            .ForMember(x => x.Category, x => x.MapFrom(x => x.Category))
            .ReverseMap();
        CreateMap<Specification, Application.Model.Models.Specification>().ReverseMap();


        //CreateMap<Specification, Application.Model.Models.Specification>()
        //    .ForMember(dest => dest.Value, opt => opt.MapFrom<Resolver>());

        CreateMap<Component, Application.Model.Models.Component>()
            .ForMember(dest => dest.SpecificationValue, opt => opt.MapFrom(src => src.SpecificationValues));

        CreateMap<SpecificationValue, Application.Model.Models.Specification>().ReverseMap();
        CreateMap<SpecificationValue, Application.Model.Models.SpecificationValue>().ReverseMap();

        CreateMap<Specification, Application.Model.Models.SpecificationValue>()
            .ForMember(x=>x.SpecificationName, x=>x.MapFrom(x=>x.Name));
    }
}
