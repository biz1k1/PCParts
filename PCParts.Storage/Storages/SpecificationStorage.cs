using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PCParts.Application.AbstractionStorage;
using PCParts.Application.Model.Enum;
using PCParts.Application.Model.Models;
using PCParts.Application.Model.QueryModel;
using PCParts.Storage.Extensions;

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

    public async Task<IEnumerable<Specification>> GetSpecificationsByCategory(Guid categoryId,
        CancellationToken cancellationToken)
    {
        var specifications = await _pgContext.Specifications
            .AsNoTracking()
            .Where(x => x.CategoryId == categoryId)
            .ToArrayAsync(cancellationToken);
        return _mapper.Map<Specification[]>(specifications);
    }

    public async Task<Specification> CreateSpecification(Guid categoryId, string name,
        SpecificationDataType dataType, CancellationToken cancellationToken)
    {
        var specificationId = Guid.NewGuid();

        var specification = new Domain.Entities.Specification
        {
            Id = specificationId,
            Name = name,
            DataType = (Domain.Enum.SpecificationDataType)dataType,
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

    public async Task<Specification> GetSpecification(Guid id, string[] includes, CancellationToken cancellationToken)
    {
        var specification = await _pgContext.Specifications
            .AsNoTracking()
            .Where(x => x.Id == id)
            .ApplyInclude(includes)
            .FirstOrDefaultAsync(cancellationToken);
        return _mapper.Map<Specification>(specification);
    }

    public async Task RemoveSpecification(Specification specification, CancellationToken cancellationToken)
    {
        _pgContext.Specifications.Remove(_mapper.Map<Domain.Entities.Specification>(specification));
        await _pgContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Specification> UpdateSpecification(UpdateQuery query, CancellationToken cancellationToken)
    {
        await _pgContext.Database.ExecuteSqlRawAsync(query.Query, query.Parameters);

        var updatedSpecification = await _pgContext.Specifications
            .AsNoTracking()
            .Where(x => x.Id == query.Id)
            .FirstAsync(cancellationToken);
        return _mapper.Map<Specification>(updatedSpecification);
    }
}