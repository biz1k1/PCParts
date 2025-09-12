using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PCParts.Domain.Exceptions;

namespace PCParts.API.Extension.Middlewares;

public static class ProblemDetailsFactoryExtensions
{
    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory, HttpContext httpContext,
        DomainException domainException)
    {
        return factory.CreateProblemDetails(httpContext,
            domainException.DomainErrorCode switch
            {
                DomainErrorCode.NotFound => StatusCodes.Status404NotFound,
                DomainErrorCode.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            },
            domainException.Message);
    }

    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory, HttpContext httpContext,
        ValidationException validationException)
    {
        var modelStateDictionary = new ModelStateDictionary();
        foreach (var error in validationException.Errors)
            modelStateDictionary.AddModelError(error.PropertyName, error.ErrorCode);

        return factory.CreateValidationProblemDetails(httpContext,
            modelStateDictionary,
            StatusCodes.Status400BadRequest,
            "Validation failed");
    }
}
