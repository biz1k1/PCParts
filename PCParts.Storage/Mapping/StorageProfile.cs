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

        CreateMap<Domain.Entities.SpecificationValue, Application.Model.Models.SpecificationValue>();
        CreateMap<Domain.Entities.Specification, Application.Model.Models.Specification>()
            .ForMember(x=>x.Type,x=>x.MapFrom(x=>x.DataType))
            .ReverseMap();



    }
}
