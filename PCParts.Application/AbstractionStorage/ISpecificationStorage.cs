﻿using PCParts.Application.Model.Models;
using PCParts.Application.Model.QueryModel;
using PCParts.Domain.Enum;

namespace PCParts.Application.AbstractionStorage;

public interface ISpecificationStorage
{
    Task<Specification> GetSpecification(Guid id, CancellationToken cancellationToken);

    Task<Specification> CreateSpecification(Guid categoryId, string name,
        SpecificationDataType dataType, CancellationToken cancellationToken);

    Task<SpecificationValue> CreateSpecificationValue(Guid componentId, Guid specificationId,
        string value, CancellationToken cancellationToken);

    Task<Specification> UpdateSpecification(UpdateQuery query, CancellationToken cancellationToken);
    Task RemoveSpecification(Specification specification, CancellationToken cancellationToken);
}