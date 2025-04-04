using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace PCParts.Application.Services.ValidationService;

public class ValidationService : IValidationService
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Validate<T>(T request)
    {
        var validator = _serviceProvider.GetService<IValidator<T>>();
        if (validator == null) return;

        await validator.ValidateAndThrowAsync(request);
    }
}