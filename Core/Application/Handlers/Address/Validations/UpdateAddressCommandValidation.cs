using FluentValidation;

namespace Yu.Application.Handlers;

public class UpdateAddressCommandValidation : AbstractValidator<UpdateAddressCommand>
{
    public UpdateAddressCommandValidation()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Address ID must be greater than 0");

        RuleFor(x => x.FullAddress)
            .NotEmpty()
            .WithMessage("Full address cannot be empty")
            .MaximumLength(500)
            .WithMessage("Full address cannot be longer than 500 characters");

        RuleFor(x => x.SubDoor)
            .NotEmpty()
            .WithMessage("Sub door cannot be empty")
            .MaximumLength(50)
            .WithMessage("Sub door cannot be longer than 50 characters");

        RuleFor(x => x.Floor)
            .NotEmpty()
            .WithMessage("Floor cannot be empty")
            .MaximumLength(20)
            .WithMessage("Floor cannot be longer than 20 characters");

        RuleFor(x => x.Apartment)
            .NotEmpty()
            .WithMessage("Apartment cannot be empty")
            .MaximumLength(20)
            .WithMessage("Apartment cannot be longer than 20 characters");

        RuleFor(x => x.Intercom)
            .NotEmpty()
            .WithMessage("Intercom cannot be empty")
            .MaximumLength(50)
            .WithMessage("Intercom cannot be longer than 50 characters");

        RuleFor(x => x.Comment)
            .MaximumLength(500)
            .WithMessage("Comment cannot be longer than 500 characters");
    }
} 