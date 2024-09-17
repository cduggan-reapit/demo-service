using FluentAssertions;
using Reapit.Services.Demo.Core.UseCases.Dummies.GetDummies;
using Reapit.Services.Demo.Data.Services;
using Reapit.Services.Demo.Domain.Entities;

namespace Reapit.Services.Demo.Core.Test.UseCases.Dummies.GetDummies;

public class GetDummiesQueryHandlerTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    
    /*
     * Handle
     */

    [Fact]
    public async Task Handle_ReturnsEmptyCollection_WhenRepositoryReturnsEmptySet()
    {
        _unitOfWork.Dummies.GetAsync(Arg.Any<CancellationToken>())
            .Returns(Array.Empty<Dummy>());

        var query = GetQuery();
        var sut = CreateSut();
        var actual = await sut.Handle(query, default);
        actual.Should().BeEmpty();
    }
    
    [Fact]
    public async Task Handle_ReturnsEntities_WhenRepositoryReturnsValues()
    {
        var data = Enumerable.Range(1, 5)
            .Select(i => new Dummy
            {
                Id = new Guid($"00000000-0000-0000-0000-{i:D12}"),
                Name = $"Demo Dummy {i:D3}",
                DateCreated = DateTime.UnixEpoch.AddDays(i),
                DateModified = DateTime.UnixEpoch.AddMonths(i)
            })
            .ToList();
        
        _unitOfWork.Dummies.GetAsync(Arg.Any<CancellationToken>())
            .Returns(data);

        var query = GetQuery();
        var sut = CreateSut();
        var actual = await sut.Handle(query, default);
        actual.Should().BeEquivalentTo(data);
    }
    
    /*
     * Private methods
     */

    private GetDummiesQueryHandler CreateSut()
        => new(_unitOfWork);
    
    private static GetDummiesQuery GetQuery()
        => new();
}