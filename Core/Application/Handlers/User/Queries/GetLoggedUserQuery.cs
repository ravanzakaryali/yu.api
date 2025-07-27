namespace Yu.Application.Handlers;

public record GetLoggedUserQuery : IRequest<GetUserResponseDto>;

internal class GetLoggedUserQueryHandler(ICurrentUserService currentUserService, IUnitOfWorkService unitOfWorkService, IYuDbContext dbContext) : IRequestHandler<GetLoggedUserQuery, GetUserResponseDto>
{
    public async Task<GetUserResponseDto> Handle(GetLoggedUserQuery request, CancellationToken cancellationToken)
    {
        string? userId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated");

        // Fetch user with addresses included
        Member? member = await dbContext.Members
            .Include(m => m.Addresses)
            .FirstOrDefaultAsync(m => m.Id == userId, cancellationToken)
            ?? throw new UnauthorizedAccessException("User not found");

        IList<string> roles = await unitOfWorkService.RoleService.GetRolesByUser(member);

        AddressResponseDto? address = null;
        var addresses = member.Addresses.Where(a => !a.IsDeleted).ToList();
        
        if (addresses.Any())
        {
            // If there's only one address, return it
            if (addresses.Count == 1)
            {
                var singleAddress = addresses.First();
                address = new AddressResponseDto
                {
                    Id = singleAddress.Id,
                    FullAddress = singleAddress.FullAddress,
                    SubDoor = singleAddress.SubDoor,
                    Floor = singleAddress.Floor,
                    Apartment = singleAddress.Apartment,
                    Intercom = singleAddress.Intercom,
                    Comment = singleAddress.Comment,
                    Country = singleAddress.Country,
                    IsDefault = singleAddress.IsDefault,
                    CreatedDate = singleAddress.CreatedDate
                };
            }
            else
            {
                // If there are multiple addresses, return the default one
                var defaultAddress = addresses.FirstOrDefault(a => a.IsDefault);
                if (defaultAddress != null)
                {
                    address = new AddressResponseDto
                    {
                        Id = defaultAddress.Id,
                        FullAddress = defaultAddress.FullAddress,
                        SubDoor = defaultAddress.SubDoor,
                        Floor = defaultAddress.Floor,
                        Apartment = defaultAddress.Apartment,
                        Intercom = defaultAddress.Intercom,
                        Comment = defaultAddress.Comment,
                        Country = defaultAddress.Country,
                        IsDefault = defaultAddress.IsDefault,
                        CreatedDate = defaultAddress.CreatedDate
                    };
                }
            }
        }

        return new GetUserResponseDto
        {
            FullName = member.FullName,
            PhoneNumber = member.PhoneNumber!,
            Email = member.Email,
            Roles = roles,
            Address = address
        };
    }
}