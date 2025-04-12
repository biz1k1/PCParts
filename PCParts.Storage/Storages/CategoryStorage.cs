using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PCParts.Application.AbstractionStorage;
using PCParts.Application.Model.Command;
using PCParts.Application.Model.Models;
using PCParts.Storage.Extensions;

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

        var newCategory = await _pgContext.Categories
            .AsNoTracking()
            .Where(x => x.Id == categoryId)
            .FirstAsync(cancellationToken);
        return _mapper.Map<Category>(category); ;
    }

    public async Task<IEnumerable<Category>> GetCategories(CancellationToken cancellationToken)
    {
        var categories= await _pgContext.Categories
            .AsNoTracking()
            .Include(x=>x.Components)
            .ToArrayAsync(cancellationToken);
        return _mapper.Map<IEnumerable<Category>>(categories);
    }

    public async Task<Category> GetCategory(Guid id, CancellationToken cancellationToken)
    {
        string[] includes = {"Components"};
        var category = await _pgContext.Categories
            .AsNoTracking()
            .Where(x => x.Id == id)
            .ApplyInclude(includes)
            .FirstOrDefaultAsync(cancellationToken);
        return _mapper.Map<Category>(category);
    }

    public async Task<Category> UpdateCategory(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var sql = "UPDATE \"Category\" SET \"Name\"=@Name WHERE \"Id\" =@Id;";
        await _pgContext.Database.ExecuteSqlRawAsync(
            sql,
            new NpgsqlParameter("@Name", command.Name),
            new NpgsqlParameter("@Id", command.Id));

        var category= await _pgContext.Categories
            .AsNoTracking()
            .Where(x => x.Id == command.Id)
            .ProjectTo<Category>(_mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
        return _mapper.Map<Category>(category);
    }

    public async Task RemoveCategory(Category category, CancellationToken cancellationToken)
    {
        var d = _mapper.Map<Domain.Entities.Category>(category);
        _pgContext.Categories.Remove(_mapper.Map<Domain.Entities.Category>(category));
        await _pgContext.SaveChangesAsync(cancellationToken);
    }
}