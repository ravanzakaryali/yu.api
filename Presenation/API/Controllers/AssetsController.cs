namespace Yu.API.Controllers;


[Authorize]
public class AssetsController : BaseApiController
{
    [HttpPost]
    [ProducesResponseType(typeof(AssetResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAssetAsync([FromForm] AssetRequestDto request)
    {
        return Ok(await Mediator.Send(new AssetUploadCommand(request.File)));
    }
}