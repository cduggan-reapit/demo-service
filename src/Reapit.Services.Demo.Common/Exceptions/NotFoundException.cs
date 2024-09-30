using Microsoft.AspNetCore.Mvc;

namespace Reapit.Services.Demo.Common.Exceptions;

public class NotFoundException : Exception
{
    internal const string ProblemType = "https://www.reapit.com/errors/not-found";
    internal const string ProblemTitle = "Resource Not Found";
    internal const int ProblemStatus = 404;
        
    public NotFoundException(string type, string identifier)
        : base($"{type} not found matching identifier \"{identifier}\"")
    {
    }

    public static ProblemDetails GetProblemDetails(Exception exception)
    {
        if(exception is not NotFoundException notFoundException)
            throw new Exception($"Cannot create NotFoundException problem description from exception of type {exception.GetType().Name}.");

        return new ProblemDetails
        {
            Type = ProblemType,
            Title = ProblemTitle,
            Detail = exception.Message,
            Status = ProblemStatus
        };
    }
}