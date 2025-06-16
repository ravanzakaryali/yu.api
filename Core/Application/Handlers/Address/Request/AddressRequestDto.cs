
namespace Yu.Application.DTOs;

public record CreateAddressCommand(string City, string Street, string House, string? Apartment, string? PostalCode) : IRequest;

internal class CreateAddressCommandHandler(IYuDbContext yuDbContext, ICurrentUserService currentUserService) : IRequestHandler<CreateAddressCommand>
{
    public async Task Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId
                ?? throw new UnauthorizedAccessException("User is not authenticated.");

        Member? member = await yuDbContext.Members
            .Include(m => m.Addresses)
            .FirstOrDefaultAsync(m => m.Id == userId, cancellationToken)
                ?? throw new UnauthorizedAccessException("User is not authenticated.");

        member.Addresses.Add(new Address
        {
            City = request.City,
            Street = request.Street,
            Country = "Azerbaijan",
            House = request.House,
            PostalCode = request.PostalCode
        });
        await yuDbContext.SaveChangesAsync(cancellationToken);
    }
}