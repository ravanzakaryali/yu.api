namespace Yu.API.Controllers;

public class OrdersController : BaseApiController
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateOrderAsync([FromBody] OrderRequestDto request)
    {
        return Ok(await Mediator.Send(new CreateOrderCommand(request.Comment, request.Files, request.Services)));
    }

    [HttpPost("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteOrderAsync([FromRoute] int id, [FromBody] DeleteOrderRequestDto request)
    {
        await Mediator.Send(new DeleteOrderCommand(id, request.ReasonId));
        return NoContent();
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> OrderDetailsAync([FromRoute] int id)
    {
        return Ok(await Mediator.Send(new GetOrderDetailsQuery(id)));
    }
}