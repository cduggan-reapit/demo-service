using FluentValidation;

namespace Reapit.Services.Demo.Core.UseCases.Dummies.CreateDummy;

public class CreateDummyCommandValidator : AbstractValidator<CreateDummyCommand>
{
    public CreateDummyCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("Must not be empty")
            .MaximumLength(100)
            .WithMessage("Must be fewer than 100 characters in length");
    }
}