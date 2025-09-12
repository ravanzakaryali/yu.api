namespace Yu.API.Controllers.Admin;

public class EmployeeController : BaseAdminApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponseDto<EmployeeResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployees([FromQuery] GetEmployeesFilterRequestDto? filter = null)
        => Ok(await Mediator.Send(new GetEmployeesQuery(filter)));

    [HttpPost]
    [ProducesResponseType(typeof(AddEmployeeResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddEmployee([FromBody] AddEmployeeRequestDto request)
        => Ok(await Mediator.Send(new AddEmployeeCommand(request.FullName, request.Username, request.Password)));

    [HttpPut]
    [ProducesResponseType(typeof(EmployeeResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeRequestDto request)
        => Ok(await Mediator.Send(new UpdateEmployeeCommand(request.Id, request.FullName, request.Email)));
}