using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PCParts.Application.Abstraction;
using PCParts.Application.Model.Models;
using PCParts.Application.Services.CategoryService;
using PCParts.Application.Services.ComponentService;
using PCParts.Application.Services.QueryBuilderService;
using PCParts.Application.Services.SpecificationService;
using PCParts.Application.Services.SpecificationValueService;
using PCParts.Application.Services.ValidationService;
using PCParts.Storage;
using PCParts.Storage.Mapping;
using PCParts.Storage.Storages;

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
            .AddScoped<IQueryBuilderService, QueryBuilderService>()
            .AddDbContextPool<PgContext>(options => options
                .UseNpgsql(connectionString));

        services.AddAutoMapper(config => config
            .AddMaps(Assembly.GetAssembly(typeof(PgContext))));
        services.AddAutoMapper(typeof(StorageProfile));
        services.AddValidatorsFromAssemblyContaining<Category>(includeInternalTypes: true);

        return services;
    }
}