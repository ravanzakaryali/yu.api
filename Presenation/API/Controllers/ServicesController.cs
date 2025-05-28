namespace Yu.API.Controllers;

public class ServicesController : BaseApiController
{

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateService([FromBody] ServiceRequestDto request)
        => Ok(await Mediator.Send(new CreateServiceCommand(
                                        request.Title,
                                        request.SubTitle,
                                        request.Desciption,
                                        request.ImageIds)));

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllServices()
        => Ok(await Mediator.Send(new GetAllServicesQuery()));

}