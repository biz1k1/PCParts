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

        CreateMap<SpecificationValue, Application.Model.Models.SpecificationValue>();
        CreateMap<Specification, Application.Model.Models.Specification>()
            .ForMember(x => x.Type, x => x.MapFrom(x => x.DataType))
            .ReverseMap();

        CreateMap<SpecificationValue, Application.Model.Models.SpecificationValue>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom<Resolver>());
    }
}