using System.Diagnostics.CodeAnalysis;
using Reapit.Services.Demo.Api.Controllers.Dummies.Models;

namespace Reapit.Services.Demo.Api.Controllers.Dummies.Examples;

[ExcludeFromCodeCoverage]
public static class ReadDummyModelExampleBase
{
    public static ReadDummyModel GetExample()
        => new(
            Id: "851f3e46cc664149a066fc062dc0ed8c",
            Name: "Example Dummy",
            DateCreated: new DateTime(2020, 1, 12, 15, 47, 32),
            DateModified: new DateTime(2024, 9, 3, 13, 14, 16));
}