namespace Yu.API.Controllers;


[Authorize]
public class AssetsController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateAssetAsync([FromForm] AssetRequestDto request)
    {
        return Ok(await Mediator.Send(new AssetUploadCommand(request.File)));
    }
}