using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PCParts.Application.Abstraction.Authentication;
using PCParts.Application.Abstraction.Storage;
using PCParts.Application.Model.Models;
using PCParts.Application.Services.CategoryService;
using PCParts.Application.Services.ComponentService;
using PCParts.Application.Services.PendingUserService;
using PCParts.Application.Services.SpecificationService;
using PCParts.Application.Services.SpecificationValueService;
using PCParts.Application.Services.ValidationService;
using PCParts.Storage;
using PCParts.Storage.Authentication;
using PCParts.Storage.BackgroundServices;
using PCParts.Storage.Mapping;
using PCParts.Storage.Storages;
using RabbitMQ.Client;

namespace PCParts.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServiceExtensions(this IServiceCollection services, string connectionString)
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
            .AddScoped<IPasswordHasher, PasswordHasher>()
            .AddDbContextPool<PgContext>(options => options
                .UseNpgsql(connectionString));
        services.AddHostedService<RabbitMqQueue>();
        services.AddSingleton<IConnectionFactory>(_ => new ConnectionFactory
        {
            HostName = "rabbitmq",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddAutoMapper(config => config
            .AddMaps(Assembly.GetAssembly(typeof(PgContext))));
        services.AddAutoMapper(typeof(StorageProfile));
        services.AddValidatorsFromAssemblyContaining<Category>(includeInternalTypes: true);

        return services;
    }
}