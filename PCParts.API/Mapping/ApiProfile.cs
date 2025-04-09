﻿using AutoMapper;
using PCParts.API.Model;

namespace PCParts.API.Mapping;

public class ApiProfile : Profile
{
    public ApiProfile()
    {
        CreateMap<Category, Application.Model.Models.Category>().ReverseMap();
        CreateMap<Component, Application.Model.Models.Component>().ReverseMap();
        CreateMap<Specification, Application.Model.Models.Specification>().ReverseMap();
        
        CreateMap<SpecificationValue, Application.Model.Models.SpecificationValue>().ReverseMap()
            .ForMember(x=>x.Id,x=>x.MapFrom(x=>x.Id))
            .ForMember(x=>x.Value,x=>x.MapFrom(x=>x.Value));

        CreateMap<PCParts.Application.Model.Models.SpecificationValue, PCParts.API.Model.SpecificationValue>();
        CreateMap<PCParts.Application.Model.Models.Component, PCParts.API.Model.Component>();
    }
}