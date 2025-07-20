namespace Yu.API.Controllers.Admin;

[Authorize]
public class ServicesController : BaseAdminApiController
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

}