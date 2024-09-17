using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Reapit.Services.Demo.Data.Context;
using Reapit.Services.Demo.Data.Repositories;
using Reapit.Services.Demo.Data.UnitTests.Helpers;
using Reapit.Services.Demo.Domain.Entities;

namespace Reapit.Services.Demo.Data.UnitTests.Repositories;

public class DummyRepositoryTests : DatabaseAwareTestBase
{
    /*
     * GetAsync
     */

    [Fact]
    public async Task GetAsync_ReturnsEmptyCollection_WhenDatabaseEmpty()
    {
        await using var context = await GetContextAsync();
        var sut = CreateSut(context);
        var actual = await sut.GetAsync(default);
        actual.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAsync_ReturnsCollection_WhenDatabasePopulated()
    {
        var seedData = Enumerable.Range(1, 100)
            .Select(i => new Dummy
            {
                Id = new Guid($"00000000-0000-0000-0000-{i:D12}"),
                Name = $"Demo Dummy {i:D3}",
                DateCreated = DateTime.UnixEpoch.AddDays(i),
                DateModified = DateTime.UnixEpoch.AddMonths(i)
            })
            .ToList();
        
        await using var context = await GetContextAsync();
        await context.Dummies.AddRangeAsync(seedData);
        await context.SaveChangesAsync();
        
        var sut = CreateSut(context);
        var actual = await sut.GetAsync(default);
        actual.Should().BeEquivalentTo(seedData);
    }
    
    /*
     * GetByIdAsync
     */

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenEntityNotFound()
    {
        var id = Guid.NewGuid();
        await using var context = await GetContextAsync();
        var sut = CreateSut(context);
        var actual = await sut.GetByIdAsync(id, default);
        actual.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsEntity_WhenEntityFound()
    {
        var id = new Guid("00000000-0000-0000-0000-000000000003");
        var seedData = Enumerable.Range(1, 5)
            .Select(i => new Dummy
            {
                Id = new Guid($"00000000-0000-0000-0000-{i:D12}"),
                Name = $"Demo Dummy {i:D3}",
                DateCreated = DateTime.UnixEpoch.AddDays(i),
                DateModified = DateTime.UnixEpoch.AddMonths(i)
            })
            .ToList();
        
        await using var context = await GetContextAsync();
        await context.Dummies.AddRangeAsync(seedData);
        await context.SaveChangesAsync();
        
        var sut = CreateSut(context);
        var actual = await sut.GetByIdAsync(id, default);
        actual.Should().NotBeNull();
        actual?.Id.Should().Be(id);
    }
    
    /*
     * CreateAsync
     */
    
    [Fact]
    public async Task CreateAsync_AddsEntityToChangeTracking()
    {
        var dummy = new Dummy("test name");
        
        await using var context = await GetContextAsync();
        var sut = CreateSut(context);
        await sut.CreateAsync(dummy, default);

        context.ChangeTracker.Entries().Should().HaveCount(1)
            .And.AllSatisfy(entry => entry.State.Should().Be(EntityState.Added));
    }
    
    /*
     * Private methods
     */

    private static DummyRepository CreateSut(DemoDbContext dbContext)
        => new(dbContext);
}