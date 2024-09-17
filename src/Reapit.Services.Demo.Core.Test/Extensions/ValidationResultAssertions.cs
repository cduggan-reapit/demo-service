using System.Text;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using FluentValidation.Results;

namespace Reapit.Services.Demo.Core.Test.Extensions;

public class ValidationResultAssertions 
    : ReferenceTypeAssertions<ValidationResult, ValidationResultAssertions>
{
    protected override string Identifier => "ValidationResult";
    
    public ValidationResultAssertions(ValidationResult instance)
        : base(instance)
    {
    }

    [CustomAssertion]
    public AndConstraint<ValidationResultAssertions> Pass(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.IsValid)
            .FailWith(GetFailedAssertionMessage($"{Identifier} does not indicate success."));

        return new AndConstraint<ValidationResultAssertions>(this);
    }
    
    [CustomAssertion]
    public AndConstraint<ValidationResultAssertions> HaveError(
        string propertyName, 
        bool isCaseSensitive = false, 
        string because = "", 
        params object[] becauseArgs)
    {
        var comparisonType = isCaseSensitive
            ? StringComparison.Ordinal
            : StringComparison.OrdinalIgnoreCase;
        
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.Errors.Any(error => error.PropertyName.Equals(propertyName, comparisonType)))
            .FailWith(GetFailedAssertionMessage($"{Identifier} does not contain error for property \"{propertyName}\"."));

        return new AndConstraint<ValidationResultAssertions>(this);
    }
    
    private string GetFailedAssertionMessage(string reason)
    {
        var sb = new StringBuilder();
        sb.AppendLine(reason);
        sb.AppendLine($"Validation state: {Subject.IsValid}");
        if (Subject.IsValid)
            return sb.ToString();
        
        sb.AppendLine(new string('-', 50));
        foreach (var error in Subject.Errors)
            sb.AppendLine($"  {error.PropertyName}: {error.ErrorMessage}");
        sb.AppendLine(new string('-', 50));
        return sb.ToString();
    }
}