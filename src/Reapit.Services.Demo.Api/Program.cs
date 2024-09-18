using Reapit.Packages.ErrorHandling.Middleware;
using Reapit.Services.Demo.Api.Infrastructure;
using Reapit.Services.Demo.Core;
using Reapit.Services.Demo.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddPresentationServices()
    .AddCoreServices()
    .AddDataServices()
    .AddSwaggerServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseSwaggerServices();

app.UseExceptionHandling(app.Services.GetRequiredService<ILoggerFactory>());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

/// <summary>Class description allowing test service injection.</summary>
public partial class Program { }