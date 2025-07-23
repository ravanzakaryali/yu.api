namespace Yu.API.Controllers.Admin;

[ApiController, Route("api/admin/[controller]")]
public class OrdersController : BaseAdminApiController
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(PaginatedResponseDto<OrderResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrdersAsync([FromQuery] GetOrdersFilterRequestDto? filter)
    {
        return Ok(await Mediator.Send(new GetOrdersQuery(filter)));
    }

    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(OrderDetailsResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrderDetailsAsync([FromRoute] int id)
    {
        return Ok(await Mediator.Send(new GetAdminOrderDetailsQuery(id)));
    }

    [HttpPatch("{id}/change-date")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ChangeOrderDateAsync([FromRoute] int id,
        [FromBody] UpdateOrderDateRequestDto request)
    {
        await Mediator.Send(new UpdateOrderDateCommand(id, request.Date, request.DateType));
        return NoContent();
    }


}