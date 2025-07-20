namespace Yu.API.Controllers;

public class ServicesController : BaseApiController
{

    
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ServiceResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllServices()
        => Ok(await Mediator.Send(new GetAllServicesQuery()));

}