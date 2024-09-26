using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Reapit.Services.Demo.Api.Infrastructure.Swagger;

/// <summary>Named options provider for the <see cref="SwaggerGenOptions"/> type.</summary>
public class ConfigureSwaggerGen : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    private readonly ConfigureSwaggerGenOptions _options;

    /// <summary>Initialize a new instance of the <see cref="ConfigureSwaggerGen"/> class.</summary>
    /// <param name="provider">The API version description provider.</param>
    /// <param name="settings">The settings to use when configuring swagger.</param>
    public ConfigureSwaggerGen(IApiVersionDescriptionProvider provider, IOptions<ConfigureSwaggerGenOptions> settings)
    {
        _provider = provider;
        _options = settings.Value;
    }

    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options)
    {
        var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "Reapit.*.xml", SearchOption.TopDirectoryOnly);
        foreach (var xmlFile in xmlFiles)
            options.IncludeXmlComments(xmlFile);
            
        options.ExampleFilters();
        
        foreach(var description in _provider.ApiVersionDescriptions)
            options.SwaggerDoc(name: description.GroupName, info: new OpenApiInfo { Title = _options.DocumentTitle, Version = description.ApiVersion.ToString() });
    }
    
    /// <inheritdoc />
    public void Configure(string? name, SwaggerGenOptions options)
        => Configure(options);
}