using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reapit.Services.Demo.Data.Context;
using Reapit.Services.Demo.Data.Services;

namespace Reapit.Services.Demo.Data;

public static class Startup
{
    public static WebApplicationBuilder AddDataServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DemoDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));
        
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return builder;
    }
}
