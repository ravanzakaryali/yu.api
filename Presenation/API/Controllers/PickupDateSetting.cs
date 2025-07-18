namespace Yu.API.Controllers;

public class PickupDateSettingController : BaseApiController
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CreatePickupDateSetting([FromBody] List<CreatePickupDateSettingRequestDto> request)
    {
        await Mediator.Send(new CreatePickupDateCommand(request));
        return NoContent();
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(List<PickupDateSettingResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPickupDateSettings()
        => Ok(await Mediator.Send(new GetAllPickupDateQuery()));
}