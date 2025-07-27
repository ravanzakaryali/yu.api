using FluentValidation;

namespace Yu.Application.Handlers;

public class DeleteAddressCommandValidation : AbstractValidator<DeleteAddressCommand>
{
    public DeleteAddressCommandValidation()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Address ID must be greater than 0");
    }
} 