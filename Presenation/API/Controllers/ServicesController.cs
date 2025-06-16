namespace Yu.API.Controllers;

public class ServicesController : BaseApiController
{

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ServiceResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateService([FromBody] ServiceRequestDto request)
        => Ok(await Mediator.Send(new CreateServiceCommand(
                                        request.Title,
                                        request.SubTitle,
                                        request.Desciption,
                                        request.ServiceType,
                                        request.ImageIds)));

    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ServiceResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllServices()
        => Ok(await Mediator.Send(new GetAllServicesQuery()));

}