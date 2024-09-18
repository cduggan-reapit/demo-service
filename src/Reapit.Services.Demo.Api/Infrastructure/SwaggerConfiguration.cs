using System.Reflection;
using Microsoft.OpenApi.Models;
using Reapit.Services.Demo.Api.Controllers.Dummies.Examples;
using Swashbuckle.AspNetCore.Filters;

namespace Reapit.Services.Demo.Api.Infrastructure;

/// <summary>Startup extension methods for Swagger documentation.</summary>
public static class SwaggerConfiguration
{
    /// <summary>Adds swagger services to the application builder.</summary>
    public static WebApplicationBuilder AddSwaggerServices(this WebApplicationBuilder builder)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddSwaggerExamplesFromAssemblyOf<ReadDummyModelExample>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.ExampleFilters();
    
            var info = new OpenApiInfo
            {
                Title = "Dummy API Documentation",
                Version = "v1",
                Description = "Description of the Dummy API"
            };
            
            c.SwaggerDoc("v1", info);
            
            // Include XML comments in the swagger doc+
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        return builder;
    }


    /// <summary>Adds swagger services to the request pipeline.</summary>
    public static WebApplication UseSwaggerServices(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }
}