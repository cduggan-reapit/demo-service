using Reapit.Platform.ApiVersioning;
using Reapit.Services.Demo.Api.Infrastructure;
using Reapit.Services.Demo.Core;
using Reapit.Services.Demo.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddPresentationServices()
    .AddCoreServices()
    .AddDataServices();

var app = builder.Build();

app.UsePresentationMiddleware();

// app.UseExceptionHandling(app.Services.GetRequiredService<ILoggerFactory>());

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRangedApiVersioning();
app.MapControllers();

app.Run();

/// <summary>Class description allowing test service injection.</summary>
public partial class Program { }