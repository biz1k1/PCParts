using Microsoft.EntityFrameworkCore;
using PCParts.Storage;

namespace PCParts.API.Extension.Migration;

public static class MigrationExtensions
{
    public static void ApplyMigration(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateAsyncScope();

        using var dbContext =
            scope.ServiceProvider.GetRequiredService<PgContext>();

        dbContext.Database.MigrateAsync();
    }
}