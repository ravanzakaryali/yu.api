namespace Yu.API.Controllers.Admin;

[Authorize]
public class ClientController : BaseAdminApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponseDto<ClientResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClientsAsync([FromQuery] GetClientsFilterRequestDto? filter)
        => Ok(await Mediator.Send(new GetClientsQuery(filter)));

    [HttpGet("{id}/orders")]
    [ProducesResponseType(typeof(IEnumerable<OrderResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClientOrdersAsync(string id)
        => Ok(await Mediator.Send(new GetClientOrdersQuery(id)));

    [HttpGet("{id}/promocodes")]
    [ProducesResponseType(typeof(IEnumerable<ClientPromoCodeResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClientPromoCodesAsync(string id)
        => Ok(await Mediator.Send(new GetClientPromoCodesQuery(id)));
}