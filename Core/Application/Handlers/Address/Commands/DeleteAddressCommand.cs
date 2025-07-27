namespace Yu.Application.Handlers;

public record DeleteAddressCommand(int Id) : IRequest;

internal class DeleteAddressCommandHandler(IYuDbContext yuDbContext, ICurrentUserService currentUserService) : IRequestHandler<DeleteAddressCommand>
{
    public async Task Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId
                ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var address = await yuDbContext.Addresses
            .FirstOrDefaultAsync(a => a.Id == request.Id && a.UserId == userId, cancellationToken)
                ?? throw new NotFoundException(nameof(Address), request.Id);

        address.IsDeleted = true;

        await yuDbContext.SaveChangesAsync(cancellationToken);
    }
} 