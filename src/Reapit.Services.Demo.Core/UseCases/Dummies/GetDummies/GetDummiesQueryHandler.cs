using MediatR;
using Reapit.Services.Demo.Data.Services;
using Reapit.Services.Demo.Domain.Entities;

namespace Reapit.Services.Demo.Core.UseCases.Dummies.GetDummies;

public class GetDummiesQueryHandler : IRequestHandler<GetDummiesQuery, IEnumerable<Dummy>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDummiesQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<IEnumerable<Dummy>> Handle(GetDummiesQuery request, CancellationToken cancellationToken)
        => await _unitOfWork.Dummies.GetAsync(cancellationToken);
}