using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Reapit.Platform.ApiVersioning;
using Reapit.Platform.ApiVersioning.Options;
using Reapit.Platform.ErrorHandling;
using Reapit.Services.Demo.Api.Infrastructure.Swagger;

namespace Reapit.Services.Demo.Api.Infrastructure;

/// <summary>Startup extension methods to inject services required by the presentation layer.</summary>
[ExcludeFromCodeCoverage]
public static class Startup
{
    /// <summary>Adds services required by the presentation layer to an application builder.</summary>
    public static WebApplicationBuilder AddPresentationServices(this WebApplicationBuilder builder)
    {
        // Automapper
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        // API Versioning
        builder.Services.AddRangedApiVersioning(typeof(Program).Assembly, new VersionedApiOptions { ApiVersionHeader = "x-api-version" });

        // Swagger things
        builder.Services.AddSwaggerServices(typeof(Program).Assembly,
            a =>
            {
                a.ApiVersionHeader = "x-api-version";
                a.DocumentTitle = "Reapit Demo API";
            });
        
        // Register controllers
        builder.Services.AddControllers();

        // Error handling (IProblemDetailsFactory)
        builder.Services.AddErrorHandlingServices();
        
        return builder;
    }

    /// <summary>Adds middleware required by the presentation layer to an application builder.</summary>
    public static IApplicationBuilder UsePresentationMiddleware(this IApplicationBuilder app)
    {
        app.RegisterApplicationExceptions();
        app.UseErrorHandlingServices();
        
        app.UseSwaggerServices();
        return app;
    }
}