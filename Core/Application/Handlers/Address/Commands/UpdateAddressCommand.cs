namespace Yu.Application.Handlers;

public record UpdateAddressCommand(int Id, string FullAddress, string Street, string SubDoor, string Floor, string Apartment, string Intercom, string Comment) : IRequest;

internal class UpdateAddressCommandHandler(IYuDbContext yuDbContext, ICurrentUserService currentUserService) : IRequestHandler<UpdateAddressCommand>
{
    public async Task Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId
                ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var address = await yuDbContext.Addresses
            .FirstOrDefaultAsync(a => a.Id == request.Id && a.UserId == userId, cancellationToken)
                ?? throw new NotFoundException(nameof(Address), request.Id);

        address.FullAddress = request.FullAddress;
        address.Street = request.Street;
        address.SubDoor = request.SubDoor;
        address.Floor = request.Floor;
        address.Apartment = request.Apartment;
        address.Intercom = request.Intercom;
        address.Comment = request.Comment;

        await yuDbContext.SaveChangesAsync(cancellationToken);
    }
} 