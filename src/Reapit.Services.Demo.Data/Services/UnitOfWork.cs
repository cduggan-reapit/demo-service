using Reapit.Services.Demo.Data.Context;
using Reapit.Services.Demo.Data.Repositories;

namespace Reapit.Services.Demo.Data.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly DemoDbContext _context;

    private DummyRepository? _dummyRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UnitOfWork(DemoDbContext context)
        => _context = context;
    
    public DummyRepository Dummies 
        => _dummyRepository ??= new DummyRepository(_context);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await _context.SaveChangesAsync(cancellationToken);
}