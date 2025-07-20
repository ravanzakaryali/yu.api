using Yu.Application.Handlers;
using Yu.Application.DTOs;

namespace Yu.API.Controllers.Admin;

public class PromoCodeController : BaseAdminApiController
{
    [HttpGet]
    public async Task<IActionResult> GetPromoCodes([FromQuery] GetPromoCodesFilterRequestDto? filter)
        => Ok(await Mediator.Send(new GetPromoCodesQuery(filter)));

    [HttpPost]
    public async Task<IActionResult> CreatePromoCode([FromBody] CreatePromoCodeRequestDto request)
        => Ok(await Mediator.Send(new CreatePromoCodeCommand(request)));
}