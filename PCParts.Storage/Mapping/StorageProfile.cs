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


        CreateMap<Specification, Application.Model.Models.Specification>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom<Resolver>());

        // Маппинг для Component -> ComponentDTO с вложенными SpecificationDTO
        CreateMap<Component, Application.Model.Models.Component>()
            .ForMember(dest => dest.Specifications, opt => opt.MapFrom(src => src.Specifications));
    }
}
