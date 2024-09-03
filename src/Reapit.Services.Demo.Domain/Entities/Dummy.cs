using Reapit.Services.Demo.Common.Temporal;
using Reapit.Services.Demo.Domain.Entities.Abstract;

namespace Reapit.Services.Demo.Domain.Entities;

public class Dummy : EntityBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Dummy"/> class.
    /// </summary>
    public Dummy()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dummy"/> class.
    /// </summary>
    /// <param name="name">The name of the Dummy.</param>
    public Dummy(string name)
    {
        Name = name;
        DateCreated = DateTimeOffsetProvider.Now.UtcDateTime;
        DateModified = DateTimeOffsetProvider.Now.UtcDateTime;
    }
    
    /// <summary>The name of the Dummy.</summary>
    public string Name { get; set; } = default!;
    
    /// <summary>The date and time on which the Dummy was created.</summary>
    /// <remarks>Represents a date and time in UTC.</remarks>
    public DateTime DateCreated { get; set; }
    
    /// <summary>The date and time on which the Dummy was last modified.</summary>
    /// <remarks>Represents a date and time in UTC.</remarks>
    public DateTime DateModified { get; set; }
}