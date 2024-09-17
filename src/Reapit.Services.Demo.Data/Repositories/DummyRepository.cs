using Microsoft.EntityFrameworkCore;
using Reapit.Services.Demo.Data.Context;
using Reapit.Services.Demo.Domain.Entities;

namespace Reapit.Services.Demo.Data.Repositories;

public class DummyRepository : IDummyRepository
{
    private readonly DemoDbContext _context;

    public DummyRepository(DemoDbContext context)
        => _context = context;

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
    }
}