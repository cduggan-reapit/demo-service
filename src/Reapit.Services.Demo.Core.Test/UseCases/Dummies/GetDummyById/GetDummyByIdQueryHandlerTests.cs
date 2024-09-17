using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Reapit.Services.Demo.Common.Exceptions;
using Reapit.Services.Demo.Core.UseCases.Dummies.GetDummyById;
using Reapit.Services.Demo.Data.Services;
using Reapit.Services.Demo.Domain.Entities;

namespace Reapit.Services.Demo.Core.Test.UseCases.Dummies.GetDummyById;

public class GetDummyByIdQueryHandlerTests
{
    private readonly IValidator<GetDummyByIdQuery> _validator = Substitute.For<IValidator<GetDummyByIdQuery>>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    
    /*
     * Handle
     */
    
    [Fact]
    public async Task Handle_ThrowsValidationException_WhenValidationFailed()
    {
        _validator.ValidateAsync(Arg.Any<GetDummyByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[] { new ValidationFailure("propertyName", "errorMessage") }));

        var command = GetQuery();
        var sut = CreateSut();
        var action = () => sut.Handle(command, default);
        await action.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenResourceNotFound()
    {
        _validator.ValidateAsync(Arg.Any<GetDummyByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        _unitOfWork.Dummies.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Dummy?>(null));

        var query = GetQuery();
        var sut = CreateSut();
        var action = () => sut.Handle(query, default);
        await action.Should().ThrowAsync<NotFoundException>();
    }
    
    [Fact]
    public async Task Handle_ReturnsEntities_WhenRepositoryReturnsValues()
    {
        var dummy = new Dummy("test dummy");
        
        _validator.ValidateAsync(Arg.Any<GetDummyByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        _unitOfWork.Dummies.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(dummy);

        var query = GetQuery();
        var sut = CreateSut();
        var actual = await sut.Handle(query, default);
        actual.Should().BeEquivalentTo(dummy);
    }
    
    /*
     * Private methods
     */

    private GetDummyByIdQueryHandler CreateSut()
        => new(_validator, _unitOfWork);
    
    private static GetDummyByIdQuery GetQuery(string id = "00000000000000000000000000000001")
        => new(id);
}