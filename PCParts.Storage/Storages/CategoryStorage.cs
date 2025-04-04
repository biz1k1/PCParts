using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PCParts.Application.AbstractionStorage;
using PCParts.Application.Model.Command;
using PCParts.Application.Model.Models;

namespace PCParts.Storage.Storages;

public class CategoryStorage : ICategoryStorage
{
    private readonly IMapper _mapper;
    private readonly PgContext _pgContext;

    public CategoryStorage(
        PgContext pgContext,
        IMapper mapper)
    {
        _pgContext = pgContext;
        _mapper = mapper;
    }

    public async Task<Category> CreateCategory(string name,
        CancellationToken cancellationToken)
    {
        var categoryId = Guid.NewGuid();
        var category = new Domain.Entities.Category
        {
            Id = categoryId,
            Name = name
        };

        await _pgContext.Categories.AddAsync(category, cancellationToken);
        await _pgContext.SaveChangesAsync(cancellationToken);

        return await _pgContext.Categories
            .AsNoTracking()
            .Where(x => x.Id == categoryId)
            .ProjectTo<Category>(_mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetCategories(CancellationToken cancellationToken)
    {
        return await _pgContext.Categories
            .AsNoTracking()
            .ProjectTo<Category>(_mapper.ConfigurationProvider)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Category?> GetCategory(Guid id, CancellationToken cancellationToken)
    {
        return await _pgContext.Categories
            .Where(x => x.Id == id)
            .ProjectTo<Category>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Category> UpdateCategory(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var sql = "UPDATE \"Category\" SET \"Name\"=@Name WHERE \"Id\" =@Id;";
        await _pgContext.Database.ExecuteSqlRawAsync(
            sql,
            new NpgsqlParameter("@Name", command.Name),
            new NpgsqlParameter("@Id", command.Id));

        return await _pgContext.Categories
            .AsNoTracking()
            .Where(x => x.Id == command.Id)
            .ProjectTo<Category>(_mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }

    public async Task RemoveCategory(Category category, CancellationToken cancellationToken)
    {
        _pgContext.Categories.Remove(_mapper.Map<Domain.Entities.Category>(category));
        await _pgContext.SaveChangesAsync(cancellationToken);
    }
}