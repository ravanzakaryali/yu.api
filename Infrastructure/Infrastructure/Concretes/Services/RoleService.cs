namespace Yu.Infrastructure.Concretes;

public class RoleService(UserManager<User> userManager) : IRoleService
{
    public async Task<IList<string>> GetRolesByUser(User user)
    => await userManager.GetRolesAsync(user);
}