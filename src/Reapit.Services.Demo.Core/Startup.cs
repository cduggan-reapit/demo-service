using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Reapit.Services.Demo.Data;

namespace Reapit.Services.Demo.Core;

public static class Startup
{
    public static WebApplicationBuilder AddCoreServices(this WebApplicationBuilder builder)
    {
        // These can't reference static classes (like Startup) so just needs to point at any class in this assembly
        builder.Services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssemblyContaining<UseCases.Dummies.CreateDummy.CreateDummyCommandHandler>());

        builder.Services.AddValidatorsFromAssemblyContaining(typeof(UseCases.Dummies.CreateDummy.CreateDummyCommandValidator));

        builder.AddDataServices();
        
        return builder;
    }
}
