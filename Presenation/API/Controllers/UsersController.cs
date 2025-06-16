namespace Yu.API.Controllers;

public class UsersController : BaseApiController
{
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(GetUserResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentUser()
        => Ok(await Mediator.Send(new GetLoggedUserQuery()));

    [HttpPost("confirm-code")]
    [ProducesResponseType(typeof(GetUserResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ConfirmCode([FromBody] ConfirmCodeRequest request)
        => Ok(await Mediator.Send(new ConfirmCodeCommand(request.PhoneNumber, request.Code)));

    [HttpPost("delete")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteUser()
    {
        await Mediator.Send(new DeleteLoggedUserCommand());
        return NoContent();
    }

    [HttpPost("address")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddAddress([FromBody] AddressRequestDto request)
    {
        await Mediator.Send(new CreateAddressCommand(request.City, request.Street, request.House, request.Apartment, request.PostalCode));
        return NoContent();
    }

    [HttpPatch]
    [Authorize]
    [ProducesResponseType(typeof(GetUserResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequestDto request)
        => Ok(await Mediator.Send(new UpdateUserCommand(request.Email, request.FullName)));
}