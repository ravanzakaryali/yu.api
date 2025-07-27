
namespace Yu.Application.DTOs;

public record CreateAddressCommand(string FullAddress, string Street, string SubDoor, string Floor, string Apartment, string Intercom, string Comment) : IRequest;

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
            FullAddress = request.FullAddress,
            Street = request.Street,
            SubDoor = request.SubDoor,
            Floor = request.Floor,
            Apartment = request.Apartment,
            Intercom = request.Intercom,
            Comment = request.Comment,
            Country = "Az",
            UserId = member.Id
        });
        await yuDbContext.SaveChangesAsync(cancellationToken);
    }
}

public record GetUserAddressesQuery : IRequest<IEnumerable<AddressResponseDto>>;

internal class GetUserAddressesQueryHandler(IYuDbContext yuDbContext, ICurrentUserService currentUserService) : IRequestHandler<GetUserAddressesQuery, IEnumerable<AddressResponseDto>>
{
    public async Task<IEnumerable<AddressResponseDto>> Handle(GetUserAddressesQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId
                ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var addresses = await yuDbContext.Addresses
            .Where(a => a.UserId == userId && !a.IsDeleted)
            .Select(a => new AddressResponseDto
            {
                Id = a.Id,
                FullAddress = a.FullAddress,
                Street = a.Street,
                SubDoor = a.SubDoor,
                Floor = a.Floor,
                Apartment = a.Apartment,
                Intercom = a.Intercom,
                Comment = a.Comment,
                Country = a.Country,
                CreatedDate = a.CreatedDate
            })
            .ToListAsync(cancellationToken);

        return addresses;
    }
}