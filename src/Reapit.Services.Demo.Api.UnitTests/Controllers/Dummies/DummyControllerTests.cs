using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using Reapit.Services.Demo.Api.Controllers.Dummies;
using Reapit.Services.Demo.Api.Controllers.Dummies.Models;
using Reapit.Services.Demo.Common.Temporal;
using Reapit.Services.Demo.Core.UseCases.Dummies.CreateDummy;
using Reapit.Services.Demo.Core.UseCases.Dummies.GetDummies;
using Reapit.Services.Demo.Core.UseCases.Dummies.GetDummyById;
using Reapit.Services.Demo.Domain.Entities;

namespace Reapit.Services.Demo.Api.UnitTests.Controllers.Dummies;

/// <summary>
/// Unit tests for the dummy controller class. This should focus on logic within the controller that does not depend
/// on integration (e.g. services throwing exceptions) or middleware (e.g. authentication/authorization). 
/// </summary>
public class DummyControllerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();

    private readonly IMapper _mapper = new MapperConfiguration(cfg
            => cfg.AddProfile(typeof(DummyProfile)))
        .CreateMapper();

    /*
     * GetDummies
     */
    
    [Fact]
    public async Task GetDummies_ReturnsOk_WhenNothingReturned()
    {
        var data = Array.Empty<Dummy>();
        _mediator.Send(Arg.Any<GetDummiesQuery>(), Arg.Any<CancellationToken>())
            .Returns(data);
        
        var sut = CreateSut();
        var result = await sut.GetDummies() as OkObjectResult;
        result?.StatusCode.Should().Be(200);
        
        var actual = result?.Value as IEnumerable<ReadDummyModel>;
        actual.Should().BeEmpty();
    }

    [Fact]
    public async Task GetDummies_ReturnsEmptyCollection_WhenDummiesFound()
    {
        var fixtureDate = new DateTimeOffset(2020, 1, 26, 10, 45, 15, TimeSpan.Zero);
        using var dateProvider = new DateTimeOffsetProviderContext(fixtureDate);
        
        var data = Enumerable.Range(1, 5)
            .Select(i => new Dummy($"test-dummy-{i}") { Id = new Guid($"{i:D32}") });

        _mediator.Send(Arg.Any<GetDummiesQuery>(), Arg.Any<CancellationToken>())
            .Returns(data);

        var expected = Enumerable.Range(1, 5)
            .Select(i => new ReadDummyModel($"{i:D32}", $"test-dummy-{i}", fixtureDate.DateTime, fixtureDate.DateTime));
        
        var sut = CreateSut();
        var result = await sut.GetDummies() as OkObjectResult;
        result?.StatusCode.Should().Be(200);
        
        var actual = result?.Value as IEnumerable<ReadDummyModel>;
        actual.Should().BeEquivalentTo(expected);
    }

    /*
     * GetDummyById
     */
    
    [Fact]
    public async Task GetDummies_ReturnsDummyModel_ForMatchedObject()
    {
        var fixtureDate = new DateTimeOffset(2020, 1, 26, 10, 45, 15, TimeSpan.Zero);
        using var dateProvider = new DateTimeOffsetProviderContext(fixtureDate);
        
        var data = new Dummy("test-dummy-1") { Id = Guid.NewGuid() };

        _mediator.Send(Arg.Any<GetDummyByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(data);

        var expected = new ReadDummyModel($"{data.Id:N}", "test-dummy-1", fixtureDate.UtcDateTime, fixtureDate.UtcDateTime);
        
        var sut = CreateSut();
        var result = await sut.GetDummyById(data.Id.ToString()) as OkObjectResult;
        result?.StatusCode.Should().Be(200);
        
        var actual = result?.Value as ReadDummyModel;
        actual.Should().BeEquivalentTo(expected);
    }
    
    /*
     * CreateDummy
     */

    [Fact]
    public async Task CreateDummy_ReturnsCreated_WhenRequestSuccessful()
    {
        var fixtureDate = new DateTimeOffset(2020, 1, 26, 10, 45, 15, TimeSpan.Zero);
        using var dateProvider = new DateTimeOffsetProviderContext(fixtureDate);
        
        var data = new Dummy("test-dummy-1") { Id = Guid.NewGuid() };

        _mediator.Send(Arg.Any<CreateDummyCommand>(), Arg.Any<CancellationToken>())
            .Returns(data);

        var input = new WriteDummyModel(data.Name);
        var expected = new ReadDummyModel($"{data.Id:N}", data.Name, data.DateCreated, data.DateModified);
        var sut = CreateSut();
        var result = await sut.CreateDummy(input) as CreatedAtActionResult;
        result?.StatusCode.Should().Be(201);

        // Check the result points to the get endpoint
        result?.ActionName.Should().BeEquivalentTo(nameof(DummyController.GetDummyById));
        result?.RouteValues.Should().BeEquivalentTo(new RouteValueDictionary { { "id", expected.Id } });
        
        // Check the payload is what we expect
        var actual = result?.Value as ReadDummyModel;
        actual.Should().BeEquivalentTo(expected);
        
        // Check that the command is constructed correctly.  We could also check this in a test class specific to the
        // automapper profile for this type.  That's likely more suitable when models become larger (as it allows tests
        // to focus on the mapping itself rather than being concerned with the surrounding fluff)
        await _mediator.Received(1).Send(Arg.Is<CreateDummyCommand>(cmd => cmd.Name == input.Name), Arg.Any<CancellationToken>());
    }
    
    /*
     * Private methods
     */

    private DummyController CreateSut() 
        => new(_mapper, _mediator);
}