namespace PCParts.Application.Services.ValidationService;

public interface IValidationService
{
    Task Validate<T>(T request);
}