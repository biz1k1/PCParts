using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PCParts.Storage.Tests;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    //private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder().Build();

    //protected override void ConfigureWebHost(IWebHostBuilder builder)
    //{
    //    var configuration = new ConfigurationBuilder()
    //        .AddInMemoryCollection(new Dictionary<string, string>
    //        {
    //            ["ConnectionStrings:Postgres"] = _dbContainer.GetConnectionString()
    //        }).Build();
    //    builder.UseConfiguration(configuration);
    //    base.ConfigureWebHost(builder);
    //}

    //public async Task InitializeAsync()
    //{
    //    await _dbContainer.StartAsync();
    //    var pgContext = new PgContext(new DbContextOptionsBuilder<PgContext>()
    //        .UseNpgsql(_dbContainer.GetConnectionString()).Options);
    //    await pgContext.Database.MigrateAsync();
    //}

    //public new async Task DisposeAsync() => await _dbContainer.DisposeAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string>
            {
                {
                    "ConnectionStrings:DefaultConnection",
                    "Host=localhost; Username=postgres; Password=postgres;Port=5432;Database=pcparts;"
                }
            });
        });
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddJsonFile("appsettings.json", false); // Принудительно загружаем настройки
        });

        builder.ConfigureServices(services =>
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PgContext>();

            // Применяем миграции перед запуском тестов
            //db.Database.Migrate();
        });
        builder.UseEnvironment("Testing");
    }
}