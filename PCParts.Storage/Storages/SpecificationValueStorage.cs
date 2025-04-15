using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PCParts.Application.AbstractionStorage;
using PCParts.Application.Model.Command;
using PCParts.Application.Model.Models;
using PCParts.Application.Model.QueryModel;
using PCParts.Storage.Extensions;

namespace PCParts.Storage.Storages;

public class SpecificationValueStorage : ISpecificationValueStorage
{
    private readonly IMapper _mapper;
    private readonly PgContext _pgContext;

    public SpecificationValueStorage(
        PgContext pgContext,
        IMapper mapper)
    {
        _pgContext = pgContext;
        _mapper = mapper;
    }

    public async Task<SpecificationValue> GetSpecificationValue(
        Guid specificationValueId, string[] includes, CancellationToken cancellationToken)
    {
        var specificationValue = await _pgContext.SpecificationsValue
            .AsNoTracking()
            .Where(x => x.Id == specificationValueId)
            .ApplyInclude(includes)
            .FirstOrDefaultAsync(cancellationToken);
        return _mapper.Map<SpecificationValue>(specificationValue);
    }

    public async Task<IEnumerable<SpecificationValue>> GetSpecificationsValue(Guid specificationId,
        CancellationToken cancellationToken)
    {
        var component = await _pgContext.SpecificationsValue
            .AsNoTracking()
            .Where(x => x.SpecificationId == specificationId)
            .ToArrayAsync(cancellationToken);
        return _mapper.Map<SpecificationValue[]>(component);
    }

    public async Task<SpecificationValue> CreateSpecificationValue(Guid componentId,
        ICollection<CreateSpecificationValueCommand> command, CancellationToken cancellationToken)
    {
        var specificationValues = command.Select(dto => new Domain.Entities.SpecificationValue
        {
            Id = Guid.NewGuid(),
            Value = dto.Value,
            SpecificationId = dto.SpecificationId,
            ComponentId = componentId
        }).ToList();

        await _pgContext.SpecificationsValue.AddRangeAsync(specificationValues, cancellationToken);
        await _pgContext.SaveChangesAsync(cancellationToken);

        return await _pgContext.Components
            .AsNoTracking()
            .Where(x => x.Id ==componentId)
            .Select(x=>x.SpecificationValues)
            .ProjectTo<SpecificationValue>(_mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }

    public async Task<SpecificationValue> UpdateSpecificationValue(UpdateQuery query,
        CancellationToken cancellationToken)
    {
        await _pgContext.Database.ExecuteSqlRawAsync(query.Query, query.Parameters);

        var updatedSpecificationValue = await _pgContext.SpecificationsValue
            .AsNoTracking()
            .Where(x => x.Id == query.Id)
            .Include(x => x.Specification)
            .FirstAsync(cancellationToken);
        return _mapper.Map<SpecificationValue>(updatedSpecificationValue);
    }
}