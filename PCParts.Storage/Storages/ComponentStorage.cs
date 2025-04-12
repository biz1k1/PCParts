using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PCParts.Application.AbstractionStorage;
using PCParts.Application.Model.Models;
using PCParts.Application.Model.QueryModel;

namespace PCParts.Storage.Storages;

public class ComponentStorage : IComponentStorage
{
    private readonly IMapper _mapper;
    private readonly PgContext _pgContext;

    public ComponentStorage(
        PgContext pgContext,
        IMapper mapper)
    {
        _pgContext = pgContext;
        _mapper = mapper;
    }

    public async Task<Component> CreateComponent(string name, Guid categoryId, CancellationToken cancellationToken)
    {
        var componentId = Guid.NewGuid();

        var component = new Domain.Entities.Component
        {
            Id = componentId,
            Name = name,
            CategoryId = categoryId
        };

        await _pgContext.Components.AddAsync(component, cancellationToken);
        await _pgContext.SaveChangesAsync(cancellationToken);

        return await _pgContext.Components
            .AsNoTracking()
            .Where(x => x.Id == componentId)
            .ProjectTo<Component>(_mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }

    public async Task<Component> GetComponent(Guid componentId, CancellationToken cancellationToken)
    {
        var component = await _pgContext.Components
            .AsNoTracking()
            .Where(x => x.Id == componentId)
            .Include(x => x.SpecificationValues)
            .ThenInclude(x => x.Specification)
            .FirstOrDefaultAsync(x => x.Id == componentId, cancellationToken);
        return _mapper.Map<Component>(component);
    }

    public async Task<IEnumerable<Component>> GetComponents(CancellationToken cancellationToken)
    {
        var components = await _pgContext.Components
            .AsNoTracking()
            .Include(x => x.SpecificationValues)
            .ThenInclude(x => x.Specification)
            .ToArrayAsync(cancellationToken);
        return _mapper.Map<Component[]>(components);
    }

    public async Task<Component> UpdateComponent(UpdateQuery query, CancellationToken cancellationToken)
    {
        await _pgContext.Database.ExecuteSqlRawAsync(query.Query, query.Parameters);
        return await _pgContext.Components
            .AsNoTracking()
            .Where(x => x.Id == query.Id)
            .ProjectTo<Component>(_mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }

    public async Task RemoveComponent(Component component, CancellationToken cancellationToken)
    {
        _pgContext.Components.Remove(_mapper.Map<Domain.Entities.Component>(component));
        await _pgContext.SaveChangesAsync(cancellationToken);
    }
}