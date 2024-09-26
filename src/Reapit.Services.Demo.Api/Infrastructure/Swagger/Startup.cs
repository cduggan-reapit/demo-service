using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Swashbuckle.AspNetCore.Filters;

namespace Reapit.Services.Demo.Api.Infrastructure.Swagger;

/// <summary>Startup extensions for Swagger configuration.</summary>
public static class Startup
{
    /// <summary>Adds swagger services to the application container.</summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assembly">The assembly containing controllers to document.</param>
    /// <param name="swaggerOptions">The swagger configuration.</param>
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services, Assembly assembly, Action<ConfigureSwaggerGenOptions> swaggerOptions)
    {
        services.Configure(swaggerOptions);

        // This is managed by ConfigureSwaggerGenOptions
        services.AddSwaggerGen(_ => { });
        
        services.AddSwaggerExamplesFromAssemblies(assembly);
        services.ConfigureOptions<ConfigureSwaggerGen>();

        return services;
    }

    /// <summary><summary>Adds swagger services to the request pipeline.</summary></summary>
    /// <param name="app">The application builder.</param>
    public static IApplicationBuilder UseSwaggerServices(this IApplicationBuilder app)
    {
        app.UseSwagger();
        
        app.UseSwaggerUI(options =>
        {
            var descriptionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
            
            foreach (var description in descriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    url: $"/swagger/{description.GroupName}/swagger.json",
                    name: description.GroupName);
            }
        });

        return app;
    }
}