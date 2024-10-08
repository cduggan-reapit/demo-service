﻿using System.Text.Json.Serialization;

namespace Reapit.Services.Demo.Api.Controllers.Dummies.V1.Models;

/// <summary>Request model used when creating or updating a Dummy.</summary>
public record WriteDummyModel(
    [property: JsonPropertyName("name")] string Name);