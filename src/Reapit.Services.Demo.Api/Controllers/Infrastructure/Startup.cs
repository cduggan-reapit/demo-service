using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Reapit.Packages.Scopes;

namespace Reapit.Services.Demo.Api.Controllers.Infrastructure;

/// <summary>Startup extension methods to inject services required by the presentation layer.</summary>
[ExcludeFromCodeCoverage]
public static class Startup
{
    /// <summary>Adds services required by the presentation layer to an application builder.</summary>
    public static WebApplicationBuilder AddPresentationServices(this WebApplicationBuilder builder)
    {
        // Automapper
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        // Reapit.Packages.Scopes
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScopeServices("demo-scopes");
        
        // Request routing (we'll come back to this for versioning)
        builder.Services.AddControllers();
        
        return builder;
    }
}