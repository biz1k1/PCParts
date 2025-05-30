using Microsoft.EntityFrameworkCore;
using PCParts.Application.Storages;
using PCParts.Domain.Entities;
using PCParts.Domain.Specification.Base;
using PCParts.Storage.Extensions;

namespace PCParts.Storage.Storages;

public class SpecificationValueStorage : ISpecificationValueStorage
{
    private readonly PgContext _pgContext;

    public SpecificationValueStorage(
        PgContext pgContext)
    {
        _pgContext = pgContext;
    }

    public async Task<SpecificationValue?> GetSpecificationValue(Guid specificationValueId,
        ISpecification<SpecificationValue> spec, CancellationToken cancellationToken)
    {
        return await _pgContext.SpecificationsValue
            .AsNoTracking()
            .Where(x => x.Id == specificationValueId)
            .ApplySpecification(spec)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<SpecificationValue> CreateSpecificationValue(Guid componentId,
        IEnumerable<SpecificationValue> entity, CancellationToken cancellationToken)
    {
        var specificationValues = entity.Select(dto => new SpecificationValue
        {
            Id = Guid.NewGuid(),
            Value = dto.Value,
            SpecificationId = dto.Id,
            ComponentId = componentId
        }).ToList();

        await _pgContext.SpecificationsValue.AddRangeAsync(specificationValues, cancellationToken);
        await _pgContext.SaveChangesAsync(cancellationToken);

        return await _pgContext.Components
            .Where(x => x.Id == componentId)
            .SelectMany(x => x.SpecificationValues)
            .FirstAsync(cancellationToken);
    }

    public async Task<SpecificationValue> UpdateSpecificationValue(SpecificationValue specificationValue,
        Dictionary<string, object> changes, CancellationToken cancellationToken)
    {
        _pgContext.Attach(specificationValue);

        foreach (var (propName, value) in changes)
        {
            var entry = _pgContext.Entry(specificationValue).Property(propName);
            entry.CurrentValue = value;
            entry.IsModified = true;
        }

        await _pgContext.SaveChangesAsync(cancellationToken);

        return await _pgContext.SpecificationsValue
            .AsNoTracking()
            .Where(x => x.Id == specificationValue.Id)
            .Include(x => x.Specification)
            .FirstAsync(cancellationToken);
    }

    public async Task<IEnumerable<SpecificationValue>> GetSpecificationsValue(Guid specificationId,
        CancellationToken cancellationToken)
    {
        return await _pgContext.SpecificationsValue
            .AsNoTracking()
            .Where(x => x.SpecificationId == specificationId)
            .ToArrayAsync(cancellationToken);
    }
}