using FluentValidation;
using Yu.Application.Handlers;

namespace Yu.Application.Handlers.Validations;

public class CreatePromoCodeCommandValidation : AbstractValidator<CreatePromoCodeCommand>
{
    public CreatePromoCodeCommandValidation()
    {
        RuleFor(x => x.Request.Code)
            .NotEmpty().WithMessage("Promo code is required")
            .MaximumLength(50).WithMessage("Promo code cannot exceed 50 characters")
            .Matches("^[A-Za-z0-9]+$").WithMessage("Promo code can only contain letters and numbers");

        RuleFor(x => x.Request.Type)
            .IsInEnum().WithMessage("Invalid promo code type");

        RuleFor(x => x.Request.Total)
            .GreaterThan(0).WithMessage("Total must be greater than 0");

        RuleFor(x => x.Request.MinumumAmount)
            .GreaterThan(0).When(x => x.Request.MinumumAmount.HasValue)
            .WithMessage("Minimum amount must be greater than 0 when specified");

        RuleFor(x => x.Request.MaxUsageCount)
            .GreaterThan(0).When(x => x.Request.MaxUsageCount.HasValue)
            .WithMessage("Max usage count must be greater than 0 when specified");

        RuleFor(x => x.Request.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .GreaterThan(DateTime.UtcNow).WithMessage("Start date must be in the future");

        RuleFor(x => x.Request.EndDate)
            .GreaterThan(x => x.Request.StartDate).When(x => x.Request.EndDate.HasValue)
            .WithMessage("End date must be after start date");
    }
} 