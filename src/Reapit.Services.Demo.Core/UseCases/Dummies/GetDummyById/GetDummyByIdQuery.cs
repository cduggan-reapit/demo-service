using MediatR;
using Reapit.Services.Demo.Domain.Entities;

namespace Reapit.Services.Demo.Core.UseCases.Dummies.GetDummyById;

public record GetDummyByIdQuery(string Id) : IRequest<Dummy>;