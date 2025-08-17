namespace Yu.API.Controllers.Admin;


public class AuthController : BaseAdminApiController
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginMember([FromBody] LoginAdminRequestDto request)
        => Ok(await Mediator.Send(new LoginAdminCommand(request.Username, request.Password)));

}