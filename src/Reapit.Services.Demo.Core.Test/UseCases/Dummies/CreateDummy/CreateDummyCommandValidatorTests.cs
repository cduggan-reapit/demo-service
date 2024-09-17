using Reapit.Services.Demo.Core.UseCases.Dummies.CreateDummy;

namespace Reapit.Services.Demo.Core.Test.UseCases.Dummies.CreateDummy;

public class CreateDummyCommandValidatorTests
{
    [Fact]
    public async Task Validation_Passes_WhenCommandValid()
    {
        var command = GetCommand();
        var sut = CreateSut();
        var actual = await sut.ValidateAsync(command);
        actual.Should().Pass();
    }
    
    /*
     * Name
     */
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public async Task Validation_Fails_WhenNameIsEmpty(string name)
    {
        var command = GetCommand(name);
        var sut = CreateSut();
        var actual = await sut.ValidateAsync(command);
        actual.Should().HaveError(nameof(CreateDummyCommand.Name));
    }
    
    [Fact]
    public async Task Validation_Fails_WhenNameExceedsMaximumLength()
    {
        var command = GetCommand(new string('a', 101));
        var sut = CreateSut();
        var actual = await sut.ValidateAsync(command);
        actual.Should().HaveError(nameof(CreateDummyCommand.Name));
    }
    
    /*
     * Private methods
     */

    private static CreateDummyCommandValidator CreateSut() => new();

    private static CreateDummyCommand GetCommand(string name = "valid name")
        => new(name);
}