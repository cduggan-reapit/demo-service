using FluentValidation;

namespace Reapit.Services.Demo.Core.UseCases.Dummies.GetDummyById;

public class GetDummyByIdQueryValidator : AbstractValidator<GetDummyByIdQuery>
{
    public GetDummyByIdQueryValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .WithMessage("Must not be empty")
            .Must(id => Guid.TryParse(id, out _))
            .WithMessage("Must be a valid guid");
    }
}