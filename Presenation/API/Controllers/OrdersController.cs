namespace Yu.API.Controllers;

public class OrdersController : BaseOrdersController
{
    [HttpGet("reasons")]
    [ProducesResponseType(typeof(IEnumerable<OrderReasonResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrderReasons()
        => Ok(await Mediator.Send(new GetAllOrderReasonsQuery()));

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateOrderAsync([FromBody] OrderRequestDto request)
    {
        return Ok(await Mediator.Send(new CreateOrderCommand(request.Comment, request.Files, request.Address, request.Services)));
    }

    [HttpPost("{id}/cancel")]
    [Authorize]
    public async Task<IActionResult> CancelOrderAsync([FromRoute] int id, [FromBody] DeleteOrderRequestDto request)
    {
        await Mediator.Send(new CancelOrderCommand(id, request.ReasonId));
        return NoContent();
    }

    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(OrderDetailsResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OrderDetailsAync([FromRoute] int id)
    {
        return Ok(await Mediator.Send(new GetOrderDetailsQuery(id)));
    }

    [HttpGet("pickup-date-settings")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<PickupDateSettingResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPickupDateSettingsAsync()
    {
        return Ok(await Mediator.Send(new GetPickupDateSettingsQuery()));
    }
}