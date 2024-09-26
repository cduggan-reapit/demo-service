namespace Reapit.Services.Demo.Api.Infrastructure.Swagger;

/// <summary>Configurable settings for the service Swagger definition.</summary>
public class ConfigureSwaggerGenOptions
{
    /// <summary>The name of the API as shown in Swagger.</summary>
    public string DocumentTitle { get; set; } = "Reapit Demo API";
    
    /// <summary>The base path of the API.</summary>
    public string? BasePath { get; set; } = null;

    /// <summary>The API version header property name.</summary>
    public string ApiVersionHeader { get; set; } = "x-api-version";
}