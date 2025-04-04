using Microsoft.EntityFrameworkCore;
using PCParts.Domain.Entities;

namespace PCParts.Storage;

public class PgContext : DbContext
{
    public PgContext(DbContextOptions<PgContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<Specification> Specifications { get; set; }
}