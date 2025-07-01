using Microsoft.EntityFrameworkCore;
using PCParts.Application.Abstraction.Storage;
using PCParts.Domain.Entities;
using PCParts.Domain.Enum;
using PCParts.Domain.Specification.Base;
using PCParts.Storage.Common.Extensions;

namespace PCParts.Storage.Storages;

public class SpecificationStorage : ISpecificationStorage
{
    private readonly PgContext _pgContext;

    public SpecificationStorage(
        PgContext pgContext)
    {
        _pgContext = pgContext;
    }

    public async Task<IEnumerable<Specification>> GetSpecificationsByCategory(Guid categoryId,
        CancellationToken cancellationToken)
    {
        return await _pgContext.Specifications
            .AsNoTracking()
            .Where(x => x.CategoryId == categoryId)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Specification> CreateSpecification(Guid categoryId, string name,
        SpecificationDataType dataType, CancellationToken cancellationToken)
    {
        var specificationId = Guid.NewGuid();

        var specification = new Specification
        {
            Id = specificationId,
            Name = name,
            DataType = dataType,
            CategoryId = categoryId
        };

        await _pgContext.Specifications.AddAsync(specification, cancellationToken);
        await _pgContext.SaveChangesAsync(cancellationToken);

        return await _pgContext.Specifications
            .Where(x => x.Id == specificationId)
            .FirstAsync(cancellationToken);
    }

    public async Task<Specification?> GetSpecification(Guid id, ISpecification<Specification> spec,
        CancellationToken cancellationToken)
    {
        return await _pgContext.Specifications
            .Where(x => x.Id == id)
            .ApplySpecification(spec)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task RemoveSpecification(Specification specification, CancellationToken cancellationToken)
    {
        _pgContext.Specifications.Remove(specification);
        await _pgContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Specification?> UpdateSpecification(Specification specification,
        Dictionary<string, object> changes, CancellationToken cancellationToken)
    {
        _pgContext.Specifications.Attach(specification);

        foreach (var change in changes)
        {
            var propertyEntry = _pgContext.Entry(specification).Property(change.Key);

            propertyEntry.CurrentValue = change.Value;
            propertyEntry.IsModified = true;
        }

        await _pgContext.SaveChangesAsync(cancellationToken);

        return await _pgContext.Specifications
            .Where(x => x.Id == specification.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}