using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Reapit.Platform.ErrorHandling.Services;
using Reapit.Services.Demo.Common.Exceptions;

namespace Reapit.Services.Demo.Api.Infrastructure;

/// <summary>ProblemDetail factory registrar for application exceptions.</summary>
public static class ExceptionRegistrar
{
    /// <summary>
    /// Register factory methods for exceptions defined in this project with the <see cref="IProblemDetailFactory"/>.
    /// </summary>
    /// <param name="app">The service collection</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IApplicationBuilder RegisterApplicationExceptions(this IApplicationBuilder app)
    {
        var factory = app.ApplicationServices.GetService<IProblemDetailsFactory>();
        
        if (factory is null)
            return app;
        
        factory.RegisterFactoryMethod<ValidationException>(HandleValidationException);
        factory.RegisterFactoryMethod<NotFoundException>(NotFoundException.GetProblemDetails);

        return app;
    }
    
    // I imagine this kind of extension method would _actually_ be part of whichever service/s we use to provide validation features (e.g. Reapit.Platform.Validation)
    private static ProblemDetails HandleValidationException(Exception exception)
    {
        if (exception is not ValidationException validationException)
            throw new Exception($"Cannot create ValidationException problem description from exception of type {exception.GetType().Name}.");
            
        return new ProblemDetails
        {
            Title = "Validation Failed",
            Type = "https://www.reapit.com/errors/validation",
            Detail = "One or more validation errors occurred.",
            Status = 422,
            Extensions =
            {
                {
                    "Errors", validationException.Errors.GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            keySelector: group => group.Key,
                            elementSelector: group => group.Select(item => item.ErrorMessage))
                }
            }
        };
    }
}