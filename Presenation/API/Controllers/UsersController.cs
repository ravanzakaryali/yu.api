namespace Yu.API.Controllers;

public class UsersController : BaseApiController
{
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(GetUserResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentUser()
        => Ok(await Mediator.Send(new GetLoggedUserQuery()));

    [HttpPost("delete")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteUser()
    {
        await Mediator.Send(new DeleteLoggedUserCommand());
        return NoContent();
    }

    [HttpGet("address")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<AddressResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserAddresses()
        => Ok(await Mediator.Send(new GetUserAddressesQuery()));

    [HttpPost("address")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddAddress([FromBody] AddressRequestDto request)
    {
        await Mediator.Send(new CreateAddressCommand(
                    request.FullAddress, 
                    request.Street, 
                    request.SubDoor, 
                    request.Floor, 
                    request.Apartment, 
                    request.Intercom, 
                    request.Comment));
        return NoContent();
    }

    [HttpPatch]
    [Authorize]
    [ProducesResponseType(typeof(GetUserResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequestDto request)
        => Ok(await Mediator.Send(new UpdateUserCommand(request.Email, request.FullName)));

    [HttpGet("orders/active")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<ActiveOrderResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserActiveOrders()
        => Ok(await Mediator.Send(new GetUserActiveOrdersQuery()));

}