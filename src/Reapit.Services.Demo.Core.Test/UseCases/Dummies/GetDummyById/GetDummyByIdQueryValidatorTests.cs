using Reapit.Services.Demo.Core.UseCases.Dummies.GetDummyById;

namespace Reapit.Services.Demo.Core.Test.UseCases.Dummies.GetDummyById;

public class GetDummyByIdQueryValidatorTests
{
    [Fact]
    public async Task Validation_Passes_WhenCommandValid()
    {
        var command = GetQuery();
        var sut = CreateSut();
        var actual = await sut.ValidateAsync(command);
        actual.Should().Pass();
    }
    
    /*
     * Id
     */
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public async Task Validation_Fails_WhenIdIsEmpty(string name)
    {
        var command = GetQuery(name);
        var sut = CreateSut();
        var actual = await sut.ValidateAsync(command);
        actual.Should().HaveError(nameof(GetDummyByIdQuery.Id));
    }
    
    [Fact]
    public async Task Validation_Fails_WhenIdIsNotGuid()
    {
        var command = GetQuery("lorem ipsum");
        var sut = CreateSut();
        var actual = await sut.ValidateAsync(command);
        actual.Should().HaveError(nameof(GetDummyByIdQuery.Id));
    }
    
    /*
     * Private methods
     */

    private static GetDummyByIdQueryValidator CreateSut() => new();

    private static GetDummyByIdQuery GetQuery(string id = "00000000000000000000000000000001")
        => new(id);
}