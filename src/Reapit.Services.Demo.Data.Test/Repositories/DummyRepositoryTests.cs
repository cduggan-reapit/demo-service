using FluentAssertions;
using Reapit.Services.Demo.Data.Context;
using Reapit.Services.Demo.Data.Repositories;
using Reapit.Services.Demo.Data.Test.Helpers;
using Reapit.Services.Demo.Domain.Entities;

namespace Reapit.Services.Demo.Data.Test.Repositories;

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
    public async Task<IEnumerable<Dummy>> GetAsync(CancellationToken cancellationToken)
        => await _context.Dummies.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<Dummy?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _context.Dummies.FindAsync(id, cancellationToken);

    public async Task CreateAsync(Dummy entity, CancellationToken cancellationToken)
        => await _context.AddAsync(entity, cancellationToken);

    public void UpdateAsync(Dummy entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void DeleteAsync(Dummy entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }*/
    
    /*
     * Private methods
     */

    private static DummyRepository CreateSut(DemoDbContext dbContext)
        => new(dbContext);
}