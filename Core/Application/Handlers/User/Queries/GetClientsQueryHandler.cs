namespace Yu.Application.Handlers;

public record GetClientsQuery : IRequest<IEnumerable<ClientResponseDto>>;

public class GetClientsQueryHandler(IYuDbContext dbContext) : IRequestHandler<GetClientsQuery, IEnumerable<ClientResponseDto>>
{

    public async Task<IEnumerable<ClientResponseDto>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
    {
        var clients = await dbContext.Members
            .Include(m => m.Orders)
            .Select(m => new ClientResponseDto
            {
                Id = m.Id,
                FullName = m.FullName,
                RegisterDate = m.CreatedDate,
                Phone = m.PhoneNumber ?? string.Empty,
                Email = m.Email,
                OrderCount = m.Orders.Count,
                TotalPrice = m.Orders.Sum(o => o.TotalPrice)
            })
            .ToListAsync(cancellationToken);

        return clients;
    }
}