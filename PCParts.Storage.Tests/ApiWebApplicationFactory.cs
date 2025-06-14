﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace PCParts.Storage.Tests;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder().Build();

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        var pgContext = new PgContext(new DbContextOptionsBuilder<PgContext>()
            .UseNpgsql(_dbContainer.GetConnectionString()).Options);
        await pgContext.Database.MigrateAsync();
    }

    public new async Task DisposeAsync() => await _dbContainer.DisposeAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:pgsql"] = _dbContainer.GetConnectionString()
            }).Build();
        builder.UseConfiguration(configuration);

        base.ConfigureWebHost(builder);
    }
}