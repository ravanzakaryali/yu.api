namespace Yu.API.Controllers.Admin;

public class ClothesController : BaseAdminApiController
{
    [HttpGet]
    public async Task<IActionResult> GetClothes()
    {
        return Ok(await Mediator.Send(new GetClothesAdminQuery()));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ClothingItemAdminResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateClothingItem([FromBody] CreateClothingItemRequestDto request)
    {
        return Ok(await Mediator.Send(new CreateClothingItemCommand(request)));
    }
}