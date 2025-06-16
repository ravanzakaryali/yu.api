namespace Yu.Application.Abstractions;

public interface IUnitOfWorkService
{
    ITokenService TokenService { get; }

    IIdentityService IdentityService { get; }

    IUserService UserService { get; }

    IRoleService RoleService { get; }
}