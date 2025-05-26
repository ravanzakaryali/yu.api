namespace Yu.API.Controllers;

[ApiController, Route("api/[controller]")]
public class BaseApiController : Controller
{
    private IYuDbContext? _spaceDbContext;
    protected IYuDbContext SpaceDbContext => _spaceDbContext ??= HttpContext.RequestServices.GetService<IYuDbContext>()!;

    private IMediator? _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;
}
