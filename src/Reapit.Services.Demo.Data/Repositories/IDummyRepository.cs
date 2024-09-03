using Reapit.Services.Demo.Domain.Entities;

namespace Reapit.Services.Demo.Data.Repositories;

public interface IDummyRepository
{
    public Task<IEnumerable<Dummy>> GetAsync(CancellationToken cancellationToken);
    
    public Task<Dummy?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    public Task CreateAsync(Dummy entity, CancellationToken cancellationToken);
    
    public void UpdateAsync(Dummy entity, CancellationToken cancellationToken);
    
    public void DeleteAsync(Dummy entity, CancellationToken cancellationToken);
}