namespace Yu.API.Controllers;

public class AuthController : BaseApiController
{
    [HttpPost("add-admin")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddAdmin([FromBody] AddAdminRequestDto request)
        => Ok(await Mediator.Send(new AddAdminCommand(request.FullName, request.Username, request.Password)));

    [HttpPost("login-admin")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginMember([FromBody] LoginAdminRequestDto request)
        => Ok(await Mediator.Send(new LoginAdminCommand(request.Username, request.Password)));

    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        => Ok(await Mediator.Send(new RegisterCommand(request.FullName, request.PhoneNumber)));

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        => Ok(await Mediator.Send(new LoginCommand(request.PhoneNumber)));

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout()
    {
        await Mediator.Send(new LogoutCommand());
        return NoContent();
    }
}