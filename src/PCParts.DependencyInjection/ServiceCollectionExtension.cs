using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using PCParts.Application.Abstraction.Authentication;
using PCParts.Application.Abstraction.Storage;
using PCParts.Application.Model.Models;
using PCParts.Application.Monitoring;
using PCParts.Application.Services.CategoryService;
using PCParts.Application.Services.ComponentService;
using PCParts.Application.Services.PendingUserService;
using PCParts.Application.Services.SpecificationService;
using PCParts.Application.Services.SpecificationValueService;
using PCParts.Application.Services.ValidationService;
using PCParts.Storage;
using PCParts.Storage.BackgroundServices;
using PCParts.Storage.Common.Authentication;
using PCParts.Storage.Common.Authentication.JwtProvider;
using PCParts.Storage.Common.Services.DbConnectionProvider;
using PCParts.Storage.Common.Services.Deduplication;
using PCParts.Storage.Common.Services.DomainEventReaderNotify;
using PCParts.Storage.Mapping;
using PCParts.Storage.Storages;
using RabbitMQ.Client;

namespace PCParts.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServiceExtensions(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddScoped<ICategoryStorage, CategoryStorage>()
            .AddScoped<IComponentStorage, ComponentStorage>()
            .AddScoped<ICategoryService, CategoryService>()
            .AddScoped<IComponentService, ComponentService>()
            .AddScoped<ISpecificationService, SpecificationService>()
            .AddScoped<ISpecificationStorage, SpecificationStorage>()
            .AddScoped<ISpecificationValueStorage, SpecificationValueStorage>()
            .AddScoped<ISpecificationValueService, SpecificationValueService>()
            .AddScoped<IValidationService, ValidationService>()
            .AddScoped<IDomainEventsStorage, DomainEventsStorage>()
            .AddScoped<IPendingUserService, PendingUserService>()
            .AddScoped<IPendingUserStorage, PendingUserStorage>()
            .AddScoped<IUserStorage, UserStorage>()
            .AddTransient<IPasswordHasher, PasswordHasher>()
            .AddTransient<IJwtTokenProvider, JwtTokenProvider>()
            .AddSingleton<IDeduplicationService, DeduplicationService>()
            .AddSingleton<IDomainEventReaderNotify, DomainEventReaderNotify>()
            .AddSingleton<IMemoryCache, MemoryCache>()
            .AddDbContextPool<PgContext>(options => options
                .UseNpgsql(configuration["Database:default_connection_string"]));
        services.AddHostedService<NotificationPublisher>();
        services.AddSingleton<IConnectionFactory>(_ => new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:HostName"]!,
            Port = configuration.GetValue<int>("RabbitMQ:Port"),
            UserName = configuration["RabbitMQ:UserName"]!,
            Password = configuration["RabbitMQ:Password"]!
        });
        services.AddSingleton<IDbConnectionProvider<NpgsqlConnection>>(sp =>
        {
            var connStr = configuration["Database:default_connection_string"]!;
            return new NpgsqlConnectionProvider(connStr);
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddAutoMapper(config => config
            .AddMaps(Assembly.GetAssembly(typeof(PgContext))));
        services.AddAutoMapper(typeof(StorageProfile));
        services.AddValidatorsFromAssemblyContaining<Category>(includeInternalTypes: true);
        services.AddSingleton<DomainMetrics>();
        return services;
    }
}
