namespace Yu.API.Controllers;

public class AuthController : BaseApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        => Ok(await Mediator.Send(new RegisterCommand(request.FullName, request.PhoneNumber)));

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        => Ok(await Mediator.Send(new LoginCommand(request.PhoneNumber)));

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await Mediator.Send(new LogoutCommand());
        return NoContent();
    }
}