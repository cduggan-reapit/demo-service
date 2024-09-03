namespace Reapit.Services.Demo.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException()
        : base("Resource not found")
    {
    }
}