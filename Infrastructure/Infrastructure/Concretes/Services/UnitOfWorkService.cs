namespace Yu.Infrastructure.Concretes;

public class UnitOfWorkService : IUnitOfWorkService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IConfiguration _configuration;
    
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UnitOfWorkService(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    private ITokenService? _tokenService;
    public ITokenService TokenService => _tokenService ??= new TokenService(_userManager, _configuration);

    private IIdentityService? _identityService;
    public IIdentityService IdentityService => _identityService ??= new IdentityService(_userManager, _roleManager);

    private IUserService? _userService;
    public IUserService UserService => _userService ??= new UserService(_userManager);

    private IRoleService? _roleService;
    public IRoleService RoleService => _roleService ??= new RoleService(_userManager);

    private ICurrentUserService? _currentUserService;
    public ICurrentUserService CurrentUserService => _currentUserService ??= new CurrentUserService(_httpContextAccessor);
}