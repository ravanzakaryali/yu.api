
namespace Yu.Application.DTOs;

public record CreateAddressCommand(string FullAddress, string SubDoor, string Floor, string Apartment, string Intercom, string Comment) : IRequest;

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

        // Check if this is the user's first address
        bool isFirstAddress = !member.Addresses.Any(a => !a.IsDeleted);
        
        member.Addresses.Add(new Address
        {
            FullAddress = request.FullAddress,
            SubDoor = request.SubDoor,
            Floor = request.Floor,
            Apartment = request.Apartment,
            Intercom = request.Intercom,
            Comment = request.Comment,
            Country = "Az",
            UserId = member.Id,
            IsDefault = isFirstAddress
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
                SubDoor = a.SubDoor,
                Floor = a.Floor,
                Apartment = a.Apartment,
                Intercom = a.Intercom,
                Comment = a.Comment,
                Country = a.Country,
                IsDefault = a.IsDefault,
                CreatedDate = a.CreatedDate
            })
            .ToListAsync(cancellationToken);

        return addresses;
    }
}

public record SetDefaultAddressCommand(int AddressId) : IRequest;

internal class SetDefaultAddressCommandHandler(IYuDbContext yuDbContext, ICurrentUserService currentUserService) : IRequestHandler<SetDefaultAddressCommand>
{
    public async Task Handle(SetDefaultAddressCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId
                ?? throw new UnauthorizedAccessException("User is not authenticated.");

        // First, set all user's addresses to non-default
        var userAddresses = await yuDbContext.Addresses
            .Where(a => a.UserId == userId && !a.IsDeleted)
            .ToListAsync(cancellationToken);

        foreach (var address in userAddresses)
        {
            address.IsDefault = false;
        }

        // Then set the specified address as default
        var targetAddress = await yuDbContext.Addresses
            .FirstOrDefaultAsync(a => a.Id == request.AddressId && a.UserId == userId && !a.IsDeleted, cancellationToken)
                ?? throw new NotFoundException("Address not found.");

        targetAddress.IsDefault = true;

        await yuDbContext.SaveChangesAsync(cancellationToken);
    }
}