namespace Yu.API.Controllers;

public class ServicesController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ServiceResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllServices()
        => Ok(await Mediator.Send(new GetAllServicesQuery()));

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ServiceDetailResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetServiceDetail(int id)
        => Ok(await Mediator.Send(new GetServiceDetailQuery(id)));

    [HttpGet("clothes")]
    [ProducesResponseType(typeof(IEnumerable<ClothingItemResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClothinItems()
        => Ok(await Mediator.Send(new GetClothinItemsQuery()));

}