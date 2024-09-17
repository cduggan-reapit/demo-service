using System.Reflection;
using Microsoft.OpenApi.Models;
using Reapit.Packages.ErrorHandling.Middleware;
using Reapit.Packages.Scopes;
using Reapit.Services.Demo.Api.Controllers.Dummies.Examples;
using Reapit.Services.Demo.Core;
using Reapit.Services.Demo.Data;
using Swashbuckle.AspNetCore.Filters;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddCoreServices()
    .AddDataServices();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddHttpContextAccessor();
builder.Services.AddScopeServices("demo-scopes");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerExamplesFromAssemblyOf<ReadDummyModelExample>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ExampleFilters();
    
    var info = new OpenApiInfo
    {
        Title = "Dummy API Documentation",
        Version = "v1",
        Description = "Description of the Dummy API"
    };
    
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    
    c.SwaggerDoc("v1", info);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandling(app.Services.GetRequiredService<ILoggerFactory>());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

/// <summary>Class description allowing test service injection.</summary>
public partial class Program { }