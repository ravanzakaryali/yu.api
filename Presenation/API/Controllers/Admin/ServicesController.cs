namespace Yu.API.Controllers.Admin;

[Authorize]
public class ServicesController : BaseAdminApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ServiceResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllServices()
        => Ok(await Mediator.Send(new GetAllServicesQuery()));

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ServiceResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateService([FromBody] ServiceRequestDto request)
          => Ok(await Mediator.Send(new CreateServiceCommand(
                                          request.Title,
                                          request.Tag,
                                          request.IconId,
                                          request.SubTitle,
                                          request.TagTextColor,
                                          request.TagBackgroundColor,
                                          request.ServiceType,
                                          request.ImageIds)));

}