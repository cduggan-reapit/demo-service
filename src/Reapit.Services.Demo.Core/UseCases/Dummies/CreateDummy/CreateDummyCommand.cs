using MediatR;
using Reapit.Services.Demo.Domain.Entities;

namespace Reapit.Services.Demo.Core.UseCases.Dummies.CreateDummy;

public record CreateDummyCommand(string Name) : IRequest<Dummy>;