using FluentValidation;
using MediatR;
using Reapit.Services.Demo.Common.Exceptions;
using Reapit.Services.Demo.Data.Services;
using Reapit.Services.Demo.Domain.Entities;

namespace Reapit.Services.Demo.Core.UseCases.Dummies.GetDummyById;

public class GetDummyByIdQueryHandler : IRequestHandler<GetDummyByIdQuery, Dummy>
{
    private readonly IValidator<GetDummyByIdQuery> _validator;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetDummyByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="validator"></param>
    /// <param name="unitOfWork"></param>
    public GetDummyByIdQueryHandler(
        IValidator<GetDummyByIdQuery> validator,
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Dummy> Handle(GetDummyByIdQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var id = Guid.Parse(request.Id);
        return await _unitOfWork.Dummies.GetByIdAsync(id, cancellationToken) 
                     ?? throw new NotFoundException(nameof(Dummy), request.Id);
    }
}