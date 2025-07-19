namespace Yu.API.Controllers.Admin;

[Authorize]
public class ClientController : BaseAdminApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ClientResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClientsAsync()
        => Ok(await Mediator.Send(new GetClientsQuery()));

    [HttpGet("{id}/orders")]
    [ProducesResponseType(typeof(IEnumerable<OrderResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClientOrdersAsync(string id)
        => Ok(await Mediator.Send(new GetClientOrdersQuery(id)));
}