using Reapit.Services.Demo.Data.Repositories;

namespace Reapit.Services.Demo.Data.Services;

public interface IUnitOfWork
{
    public IDummyRepository Dummies { get; }

    public Task SaveChangesAsync(CancellationToken cancellationToken);
}