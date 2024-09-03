using System.Diagnostics.CodeAnalysis;
using Reapit.Services.Demo.Api.Controllers.Dummies.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Reapit.Services.Demo.Api.Controllers.Dummies.Examples;

/// <summary>Swagger example provider for the <see cref="WriteDummyModel"/> type.</summary>
[ExcludeFromCodeCoverage]
public class WriteDummyModelExample : IExamplesProvider<WriteDummyModel>
{
    /// <inheritdoc />
    public WriteDummyModel GetExamples()
        => new(Name: "Dummy Name");
}