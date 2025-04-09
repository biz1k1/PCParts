using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PCParts.Application.AbstractionStorage;
using PCParts.Application.Model.Models;
using PCParts.Application.Model.QueryModel;
using PCParts.Domain.Enum;

namespace PCParts.Storage.Storages;

public class SpecificationStorage : ISpecificationStorage
{
    private readonly IMapper _mapper;
    private readonly PgContext _pgContext;

    public SpecificationStorage(
        PgContext pgContext,
        IMapper mapper)
    {
        _pgContext = pgContext;
        _mapper = mapper;
    }

    public async Task<Specification> CreateSpecification(Guid categoryId, string name, 
        SpecificationDataType dataType, CancellationToken cancellationToken)
    {
        var specificationId = Guid.NewGuid();

        var specification = new Domain.Entities.Specification
        {
            Id = specificationId,
            Name = name,
            DataType = dataType,
            CategoryId = categoryId
        };

        await _pgContext.Specifications.AddAsync(specification, cancellationToken);
        await _pgContext.SaveChangesAsync(cancellationToken);

        return await _pgContext.Specifications
            .AsNoTracking()
            .Where(x => x.Id == specificationId)
            .ProjectTo<Specification>(_mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }

    public async Task<SpecificationValue> CreateSpecificationValue(Guid componentId, 
        Guid specificationId, string value, CancellationToken cancellationToken)
    {
        var specificationValueId = Guid.NewGuid();

        var specification = new Domain.Entities.SpecificationValue()
        {
            Id = specificationValueId,
            Value = value,
            SpecificationId = specificationId,
            ComponentId = componentId,
        };

        await _pgContext.SpecificationsValue.AddAsync(specification, cancellationToken);
        await _pgContext.SaveChangesAsync(cancellationToken);

        return await _pgContext.SpecificationsValue
            .AsNoTracking()
            .Where(x => x.Id == specificationValueId)
            .ProjectTo<SpecificationValue>(_mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }

    public async Task<Specification> GetSpecification(Guid id, CancellationToken cancellationToken)
    {
        return await _pgContext.Specifications
            .Where(x => x.Id == id)
            .ProjectTo<Specification>(_mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }

    public async Task RemoveSpecification(Specification specification, CancellationToken cancellationToken)
    {
        _pgContext.Specifications.Remove(_mapper.Map<Domain.Entities.Specification>(specification));
        await _pgContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Specification> UpdateSpecification(UpdateQuery query, CancellationToken cancellationToken)
    {
        await _pgContext.Database.ExecuteSqlRawAsync(query.Query, query.Parameters);
        return await _pgContext.Specifications
            .AsNoTracking()
            .Where(x => x.Id == query.Id)
            .ProjectTo<Specification>(_mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}