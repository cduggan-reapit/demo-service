using Reapit.Services.Demo.Data.Repositories;

namespace Reapit.Services.Demo.Data.Services;

public interface IUnitOfWork
{
    public DummyRepository Dummies { get; }

    public Task SaveChangesAsync(CancellationToken cancellationToken);
}