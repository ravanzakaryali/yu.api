namespace Yu.API.Controllers;

public class AuthController : BaseApiController
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        => Ok(await Mediator.Send(new RegisterCommand(request.FullName, request.PhoneNumber)));

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        => Ok(await Mediator.Send(new LoginCommand(request.PhoneNumber)));

    [HttpPost("confirm-code")]
    [ProducesResponseType(typeof(GetUserResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ConfirmCode([FromBody] ConfirmCodeRequest request)
        => Ok(await Mediator.Send(new ConfirmCodeCommand(request.PhoneNumber, request.Code)));

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout()
    {
        await Mediator.Send(new LogoutCommand());
        return NoContent();
    }
}