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

    // [HttpPatch]
    // [Authorize]
    // public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
    // {
    //     var command = new UpdateUserCommand(request.Name, request.Surname, request.PhoneNumber);
    //     return Ok(await Mediator.Send(command));
    // }
}