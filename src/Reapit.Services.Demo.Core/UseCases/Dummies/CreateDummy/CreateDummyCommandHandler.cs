using FluentValidation;
using MediatR;
using Reapit.Services.Demo.Common.Identifier;
using Reapit.Services.Demo.Data.Services;
using Reapit.Services.Demo.Domain.Entities;

namespace Reapit.Services.Demo.Core.UseCases.Dummies.CreateDummy;

/// <summary>Mediator request handler for the <see cref="CreateDummyCommand"/> object.</summary>
public class CreateDummyCommandHandler : IRequestHandler<CreateDummyCommand, Dummy>
{
    private readonly IValidator<CreateDummyCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateDummyCommandHandler"/> class.
    /// </summary>
    /// <param name="validator"></param>
    /// <param name="unitOfWork"></param>
    public CreateDummyCommandHandler(
        IValidator<CreateDummyCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<Dummy> Handle(CreateDummyCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var dummy = new Dummy(request.Name);

        await _unitOfWork.Dummies.CreateAsync(dummy, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return dummy;
    }
}