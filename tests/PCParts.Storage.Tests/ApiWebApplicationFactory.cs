using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PCParts.Storage;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using DotNetEnv;
using Microsoft.AspNetCore.Mvc.Testing;

namespace PCParts.API.Tests;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder().Build();
    private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder().Build();
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _rabbitMqContainer.StartAsync();

        var pgContext = new PgContext(new DbContextOptionsBuilder<PgContext>()
            .UseNpgsql(_dbContainer.GetConnectionString()).Options);
        await pgContext.Database.MigrateAsync();
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await _dbContainer.DisposeAsync();
        await _rabbitMqContainer.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Env.Load(AppContext.BaseDirectory + "../../../../../solution-item/env/backend.env");
        builder.UseEnvironment("Testing");
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:pgsql"] = _dbContainer.GetConnectionString()! ?? throw new InvalidOperationException("Connection string missing"),
                ["Database:default_connection_string"] = _dbContainer.GetConnectionString(),
                ["RabbitMQ:Host"] = Environment.GetEnvironmentVariable("RabbitMQ__HostName"),
                ["RabbitMQ:Port"] = Environment.GetEnvironmentVariable("RabbitMQ__Port"),
                ["RabbitMQ:Username"] = Environment.GetEnvironmentVariable("RabbitMQ__UserName"),
                ["RabbitMQ:Password"] = Environment.GetEnvironmentVariable("RabbitMQ__Password"),
            }).Build();

        builder.UseConfiguration(configuration);

        base.ConfigureWebHost(builder);
    }

}
