namespace Reapit.Services.Demo.Common.Exceptions;

public class NotFoundException : Exception
{
    internal const string DefaultMessage = "Resource not found";
    
    public NotFoundException()
        : base(DefaultMessage)
    {
    }
}