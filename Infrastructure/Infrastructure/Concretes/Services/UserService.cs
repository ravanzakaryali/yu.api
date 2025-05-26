namespace Yu.Infrastructure.Concretes;

public class UserService(UserManager<User> userManager) : IUserService
{

    public async Task<User> FindById(string id)
    {
        return await userManager.FindByIdAsync(id) ??
                     throw new NotFoundException("User", id);
    }
}