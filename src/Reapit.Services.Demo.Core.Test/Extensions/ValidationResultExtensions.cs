using FluentValidation.Results;

namespace Reapit.Services.Demo.Core.Test.Extensions;

public static class ValidationResultExtensions 
{
    public static ValidationResultAssertions Should(this ValidationResult instance)
        => new(instance); 
}