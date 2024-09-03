using Reapit.Services.Demo.Domain.Entities.Abstract;

namespace Reapit.Services.Demo.Domain.Entities;

public class Dummy : EntityBase
{
    public string Name { get; set; } = string.Empty;
    
    public DateTime DateCreated { get; set; }
    
    public DateTime DateModified { get; set; }
}