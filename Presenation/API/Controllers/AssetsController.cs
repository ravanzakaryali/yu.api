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

    [HttpDelete("{assetId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAssetAsync([FromRoute] int assetId)
    {
        await Mediator.Send(new DeleteAssetCommand(assetId));
        return NoContent();
    }
}