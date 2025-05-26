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
}