﻿using PCParts.Application.Model.Models;
using PCParts.Application.Model.QueryModel;
using PCParts.Application.Model.Enum;

namespace PCParts.Application.Abstraction;

public interface ISpecificationStorage
{
    Task<Specification> GetSpecification(Guid id, string[] includes, CancellationToken cancellationToken);
    Task<IEnumerable<Specification>> GetSpecificationsByCategory(Guid categoryId, CancellationToken cancellationToken);
    Task<Specification> CreateSpecification(Guid categoryId, string name,
        SpecificationDataType dataType, CancellationToken cancellationToken);
    Task<Specification> UpdateSpecification(UpdateQuery query, CancellationToken cancellationToken);
    Task RemoveSpecification(Specification specification, CancellationToken cancellationToken);
}