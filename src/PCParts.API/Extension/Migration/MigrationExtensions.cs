using Microsoft.EntityFrameworkCore;
using Npgsql;
using PCParts.Storage;

namespace PCParts.API.Extension.Migration;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationAsync(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateAsyncScope();

        using var dbContext =
            scope.ServiceProvider.GetRequiredService<PgContext>();

        if (dbContext.Database.ProviderName == "Npgsql.EntityFrameworkCore.PostgreSQL")
        {
            try
            {
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    await dbContext.Database.MigrateAsync();
                }

                var checkTriggerSql = @"
                    SELECT COUNT(1) AS ""Value""
                    FROM pg_trigger 
                    WHERE tgname = 'domain_event_insert_trigger' ";

                var triggerCount =  await dbContext.Database
                    .SqlQueryRaw<IntResult> (checkTriggerSql)
                    .SingleAsync();

                if (triggerCount.Value == 0)
                {
                    const string createTriggerSql = @"
                    DO $$
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM pg_trigger WHERE tgname = 'domain_event_insert_trigger') THEN
                            EXECUTE '
                                CREATE OR REPLACE FUNCTION notify_new_event()
                                RETURNS trigger AS $func$
                                BEGIN
                                    PERFORM pg_notify(''new_event'', NEW.""Id""::text);
                                    RETURN NEW;
                                END;
                                $func$ LANGUAGE plpgsql;';
                            EXECUTE '
                                CREATE TRIGGER domain_event_insert_trigger
                                AFTER INSERT ON ""DomainEvents""
                                FOR EACH ROW
                                EXECUTE FUNCTION notify_new_event();';
                        END IF;
                    END;
                    $$;";

                    await dbContext.Database.ExecuteSqlRawAsync(createTriggerSql);
                }
            }
            catch (PostgresException pgEx)
            {

            }
            catch (DbUpdateException dbUpEx)
            {

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        
    }
    private class IntResult
    {
        public int Value { get; set; }
    }
}