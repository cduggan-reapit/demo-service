using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Reapit.Services.Demo.Core.UseCases.Dummies.CreateDummy;
using Reapit.Services.Demo.Data.Services;
using Reapit.Services.Demo.Domain.Entities;

namespace Reapit.Services.Demo.Core.Test.UseCases.Dummies.CreateDummy;

public class CreateDummyCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IValidator<CreateDummyCommand> _validator = Substitute.For<IValidator<CreateDummyCommand>>();
    
    /*
     * Handle
     */

    [Fact]
    public async Task Handle_ThrowsValidationException_WhenValidationFailed()
    {
        _validator.ValidateAsync(Arg.Any<CreateDummyCommand>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[] { new ValidationFailure("propertyName", "errorMessage") }));

        var command = GetCommand();
        var sut = CreateSut();
        var action = () => sut.Handle(command, default);
        await action.Should().ThrowAsync<ValidationException>();
    }
    
    [Fact]
    public async Task Handle_ReturnsEntity_WhenValidationSuccessful()
    {
        _validator.ValidateAsync(Arg.Any<CreateDummyCommand>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());
        
        _unitOfWork.Dummies.CreateAsync(Arg.Any<Dummy>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        var command = GetCommand();
        var sut = CreateSut();
        var actual = await sut.Handle(command, default);

        actual.Should().NotBeNull();
        actual.Name.Should().Be(command.Name);
    }
    
    /*
     * Private methods
     */

    private CreateDummyCommandHandler CreateSut()
        => new(_validator, _unitOfWork);
    
    private static CreateDummyCommand GetCommand(string name = "valid name")
        => new(name);
}