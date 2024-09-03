using Microsoft.EntityFrameworkCore;
using Reapit.Services.Demo.Data.Context.Configuration;
using Reapit.Services.Demo.Domain.Entities;

namespace Reapit.Services.Demo.Data.Context;

public class DemoDbContext : DbContext
{
    public DbSet<Dummy> Dummies { get; set; }

    public DemoDbContext(DbContextOptions<DemoDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
        => builder.ApplyConfiguration(new DummyConfiguration());
}