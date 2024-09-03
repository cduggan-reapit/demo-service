using MediatR;
using Reapit.Services.Demo.Domain.Entities;

namespace Reapit.Services.Demo.Core.UseCases.Dummies.GetDummies;

public record GetDummiesQuery() : IRequest<IEnumerable<Dummy>>;