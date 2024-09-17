using FluentAssertions;
using Reapit.Services.Demo.Common.Exceptions;

namespace Reapit.Services.Demo.Common.Test.Exceptions;

public class NotFoundExceptionTests
{
    /*
     * Ctor
     */

    [Fact]
    public void Ctor_InitialisesException_WithDefaultMessage()
    {
        var sut = new NotFoundException();
        sut.Message.Should().Be(NotFoundException.DefaultMessage);
    }
}