using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PCParts.Domain.Exceptions;

namespace PCParts.API.Extension.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next)
{
    private static readonly Action<ILogger, string, string, Exception> _logException =
        LoggerMessage.Define<string, string>(
            LogLevel.Error,
            new EventId(1, "UnhandledException"),
            "Error has happened with {RequestPath}, the message is {ErrorMessage}");

    private static readonly Action<ILogger, string, Exception> _logInfo =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(2, "Info"),
            "Info: {Message}");

    public async Task InvokeAsync(
        HttpContext httpContext,
        ILogger<ErrorHandlingMiddleware> logger,
        ProblemDetailsFactory problemDetailsFactory)
    {
        try
        {
            await next.Invoke(httpContext);
        }
        catch (Exception exception)
        {
            _logException(logger, httpContext.Request.Path.Value, exception.Message, exception);

            ProblemDetails problemDetails;
            switch (exception)
            {
                case ValidationException validationException:
                    problemDetails = problemDetailsFactory.CreateFrom(httpContext, validationException);
                    _logInfo(logger, validationException.Message, validationException);
                    break;
                case DomainException domainException:
                    problemDetails = problemDetailsFactory.CreateFrom(httpContext, domainException);
                    _logInfo(logger, domainException.Message, domainException);
                    break;
                default:
                    problemDetails = problemDetailsFactory.CreateProblemDetails(
                        httpContext, StatusCodes.Status500InternalServerError,
                        $"Unhandled error! Please contact us. Erorr: {exception.Message}/ {exception.InnerException}");
                    _logException(logger, httpContext.Request.Path.Value, exception.Message, exception);
                    break;
            }

            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, problemDetails.GetType());
        }
    }
}
