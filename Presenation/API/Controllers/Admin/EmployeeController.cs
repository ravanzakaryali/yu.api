namespace Yu.API.Controllers.Admin;

[Authorize]
public class EmployeeController : BaseAdminApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EmployeeResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployees()
        => Ok(await Mediator.Send(new GetEmployeesQuery()));

    [HttpPost]
    [ProducesResponseType(typeof(AddEmployeeResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddEmployee([FromBody] AddEmployeeRequestDto request)
        => Ok(await Mediator.Send(new AddEmployeeCommand(request.FullName, request.Username, request.Password)));

    [HttpPut]
    [ProducesResponseType(typeof(EmployeeResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeRequestDto request)
        => Ok(await Mediator.Send(new UpdateEmployeeCommand(request.Id, request.FullName, request.Email)));
}