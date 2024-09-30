using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Reapit.Services.Demo.Api.Extensions;

/// <summary>Extension methods for the <see cref="Exception"/> type.</summary>
public static class ExceptionExtensions
{
    /// <summary>Cast a base Exception to a subclass of Extension.</summary>
    /// <param name="exception">The exception to convert.</param>
    /// <typeparam name="TException">The type to cast to.</typeparam>
    /// <returns>An instance of TException.</returns>
    /// <exception cref="Exception">the exception is not an instance of TException.</exception>
    /// <remarks>This might be useful to provide through the Common package.</remarks>
    public static TException AsType<TException>(this Exception exception)
        where TException: Exception
    {
        if (exception is not TException castException)
            throw new Exception($"Exception of type {exception.GetType().Name} cannot be converted to {typeof(TException).Name}.");
        return castException;
    }
    
    /// <summary>
    /// Get an instance of <see cref="ProblemDetails"/> representing an Exception of type <see cref="ValidationException"/>.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <exception cref="Exception">the exception is not an instance of ValidationException.</exception>
    public static ProblemDetails GetValidationExceptionProblemDetails(this Exception exception)
    {
        var validationException = exception.AsType<ValidationException>();

        return new ProblemDetails
        {
            Title = "Validation Failed",
            Type = "https://www.reapit.com/errors/validation",
            Detail = "One or more validation errors occurred.",
            Status = 422,
            Extensions =
            {
                {
                    "errors", validationException.Errors.GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            keySelector: group => group.Key,
                            elementSelector: group => group.Select(item => item.ErrorMessage))
                }
            }
        };
    }
}