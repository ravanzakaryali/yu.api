namespace Yu.API.Controllers.Admin;

[Authorize]
public class ClientController : BaseAdminApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ClientResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClientsAsync()
        => Ok(await Mediator.Send(new GetClientsQuery()));
}