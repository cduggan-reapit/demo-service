﻿using System.Diagnostics.CodeAnalysis;
using Reapit.Services.Demo.Api.Controllers.Dummies.V1.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Reapit.Services.Demo.Api.Controllers.Dummies.V1.Examples;

/// <summary>Swagger example provider for the <see cref="ReadDummyModel"/> type.</summary>
[ExcludeFromCodeCoverage]
public class ReadDummyModelExample : IExamplesProvider<ReadDummyModel>
{
    /// <inheritdoc/>
    public ReadDummyModel GetExamples()
        => ReadDummyModelExampleBase.GetExample();
}