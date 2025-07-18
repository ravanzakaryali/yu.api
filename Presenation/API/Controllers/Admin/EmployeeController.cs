namespace Yu.API.Controllers.Admin;

[Authorize]
public class EmployeeController : BaseApiController
{
    [HttpPost("add-employee")]
    [ProducesResponseType(typeof(AddEmployeeResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddEmployee([FromBody] AddEmployeeRequestDto request)
        => Ok(await Mediator.Send(new AddEmployeeCommand(request.FullName, request.Username, request.Password)));
}