namespace Yu.API.Controllers;

public class UsersController : BaseApiController
{
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
        => Ok(await Mediator.Send(new GetLoggedUserQuery()));

    [HttpPost("confirm-code")]
    public async Task<IActionResult> ConfirmCode([FromBody] ConfirmCodeRequest request)
        => Ok(await Mediator.Send(new ConfirmCodeCommand(request.PhoneNumber, request.Code)));

    [HttpPost("delete")]
    [Authorize]
    public async Task<IActionResult> DeleteUser()
    {
        await Mediator.Send(new DeleteLoggedUserCommand());
        return NoContent();
    }

    [HttpPost("address")]
    [Authorize]
    public async Task<IActionResult> AddAddress([FromBody] AddressRequestDto request)
    {
        await Mediator.Send(new CreateAddressCommand(request.City, request.Street, request.House, request.Apartment, request.PostalCode));
        return NoContent();
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequestDto request)
        => Ok(await Mediator.Send(new UpdateUserCommand(request.Email, request.FullName)));
}