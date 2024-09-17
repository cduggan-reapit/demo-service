using AutoMapper;
using Reapit.Services.Demo.Api.Controllers.Dummies.Models;
using Reapit.Services.Demo.Core.UseCases.Dummies.CreateDummy;
using Reapit.Services.Demo.Domain.Entities;

namespace Reapit.Services.Demo.Api.Controllers.Dummies;

/// <summary>Automapper profile for Dummy models, requests, commands, and entities.</summary>
public class DummyProfile : Profile
{
    /// <summary>Initializes a new instance of the <see cref="DummyProfile"/> class.</summary>
    public DummyProfile()
    {
        CreateMap<WriteDummyModel, CreateDummyCommand>()
            .ForCtorParam(nameof(CreateDummyCommand.Name), ops => ops.MapFrom(model => model.Name));

        CreateMap<Dummy, ReadDummyModel>()
            .ForCtorParam(nameof(ReadDummyModel.Id), ops => ops.MapFrom(entity => entity.Id.ToString("N")))
            .ForCtorParam(nameof(ReadDummyModel.Name), ops => ops.MapFrom(entity => entity.Name))
            .ForCtorParam(nameof(ReadDummyModel.DateCreated), ops => ops.MapFrom(entity => entity.DateCreated))
            .ForCtorParam(nameof(ReadDummyModel.DateModified), ops => ops.MapFrom(entity => entity.DateModified));
    }
}