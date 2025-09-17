namespace Yu.API.Controllers.Admin;


public class AuthController : BaseAdminApiController
{
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginMember([FromBody] LoginAdminRequestDto request)
        => Ok(await Mediator.Send(new LoginAdminCommand(request.Username, request.Password)));

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(GetUserResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMe()
        => Ok(await Mediator.Send(new GetMeQuery()));

    [HttpPatch("me")]
    [Authorize]
    [ProducesResponseType(typeof(GetUserResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditAccount([FromBody] EditAccountRequestDto request)
        => Ok(await Mediator.Send(new EditAccountCommand(request.FullName, request.Email)));

}