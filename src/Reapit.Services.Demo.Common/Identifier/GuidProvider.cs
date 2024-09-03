namespace Reapit.Services.Demo.Common.Identifier;

/// <summary>Guid provider allowing fixture in test classes.</summary>
public class GuidProvider
{
    /// <inheritdoc cref="Guid.NewGuid"/>
    public static Guid New
        => GuidProviderContext.Current?.NewGuid ?? Guid.NewGuid();
}