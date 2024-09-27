using Microsoft.AspNetCore.Mvc;

namespace Reapit.Services.Demo.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string type, string identifier)
        : base($"{type} not found matching identifier \"{identifier}\"")
    {
    }

    public static ProblemDetails GetProblemDetails(Exception exception)
    {
        if(exception is not NotFoundException notFoundException)
            throw new Exception($"Cannot create ValidationException problem description from exception of type {exception.GetType().Name}.");

        return new ProblemDetails
        {
            Type = "https://www.reapit.com/errors/not-found",
            Title = "Resource Not Found",
            Detail = exception.Message,
            Status = 404
        };
    }
}