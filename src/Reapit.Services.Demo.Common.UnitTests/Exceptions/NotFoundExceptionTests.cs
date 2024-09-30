using FluentAssertions;
using Reapit.Services.Demo.Common.Exceptions;

namespace Reapit.Services.Demo.Common.UnitTests.Exceptions;

public class NotFoundExceptionTests
{
    /*
     * GetProblemDetails
     */

    [Fact]
    public void GetProblemDetails_ThrowsException_WhenNotNotFoundException()
    {
        var exception = new Exception("wrong exception type");
        var action = () => NotFoundException.GetProblemDetails(exception);
        action.Should().Throw<Exception>();
    }
    
    [Fact]
    public void GetProblemDetails_ReturnsProblemDetails_WhenExceptionIsNotFoundException()
    {
        const string type = "resourceType";
        const string id = "resourceId";
        var exception = new NotFoundException(type, id);
        
        var problem = NotFoundException.GetProblemDetails(exception);
        problem.Status = NotFoundException.ProblemStatus;
        problem.Type = NotFoundException.ProblemType;
        problem.Title = NotFoundException.ProblemTitle;
    }
}