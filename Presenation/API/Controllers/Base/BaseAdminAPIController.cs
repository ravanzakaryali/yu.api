namespace Yu.API.Controllers;

[Authorize, ApiController, Route("api/admin/[controller]")]

public class BaseAdminApiController : BaseApiController
{
}