using System.Diagnostics.CodeAnalysis;
using Reapit.Services.Demo.Api.Controllers.Dummies.Models;

namespace Reapit.Services.Demo.Api.Controllers.Dummies.Examples;

/// <summary>Static provider of an example <see cref="ReadDummyModel"/> object.</summary>
[ExcludeFromCodeCoverage]
public static class ReadDummyModelExampleBase
{
    /// <summary>Creates an example <see cref="ReadDummyModel"/> object.</summary>

    public static ReadDummyModel GetExample()
        => new(
            Id: "851f3e46cc664149a066fc062dc0ed8c",
            Name: "Example Dummy",
            DateCreated: new DateTime(2020, 1, 12, 15, 47, 32),
            DateModified: new DateTime(2024, 9, 3, 13, 14, 16));
}