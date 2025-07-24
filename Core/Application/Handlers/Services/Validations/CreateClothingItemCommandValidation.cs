using FluentValidation;
using Yu.Application.Abstractions;

namespace Yu.Application.Handlers;

public class CreateClothingItemCommandValidation : AbstractValidator<CreateClothingItemCommand>
{
    public CreateClothingItemCommandValidation(IYuDbContext dbContext)
    {
        RuleFor(x => x.Request.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty")
            .MaximumLength(100)
            .WithMessage("Name cannot be longer than 100 characters")
            .MustAsync(async (name, cancellation) =>
            {
                return !await dbContext.ClothingItems
                    .AnyAsync(ci => ci.Name == name && !ci.IsDeleted, cancellation);
            })
            .WithMessage("This name already exists");

        RuleFor(x => x.Request.EstimateHours)
            .GreaterThan(0)
            .WithMessage("Estimated hours must be greater than 0");

        RuleFor(x => x.Request.PriceValue)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0");

        RuleFor(x => x.Request.Currency)
            .NotEmpty()
            .WithMessage("Currency cannot be empty")
            .MaximumLength(10)
            .WithMessage("Currency cannot be longer than 10 characters");
    }
}