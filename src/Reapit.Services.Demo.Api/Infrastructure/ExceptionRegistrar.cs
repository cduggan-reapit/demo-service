using FluentValidation;
using Reapit.Platform.ErrorHandling.Services;
using Reapit.Services.Demo.Api.Extensions;
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
        
        factory.RegisterFactoryMethod<ValidationException>(exception => exception.GetValidationExceptionProblemDetails());
        factory.RegisterFactoryMethod<NotFoundException>(NotFoundException.GetProblemDetails);

        return app;
    }
    
    
}