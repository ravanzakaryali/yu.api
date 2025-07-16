using System;
using System.Linq;

namespace Yu.Infrastructure.Concretes;

public class UserService(UserManager<User> userManager, RoleManager<Role> roleManager) : IUserService
{

    public async Task<User> FindByIdAsync(string id)
    {
        return await userManager.FindByIdAsync(id) ??
                     throw new NotFoundException("User", id);
    }

    public async Task<User> AddAdminAsync(string fullName, string username, string password)
    {
        var user = new User
        {
            FullName = fullName,
            UserName = username,
            Email = username // Email tələb olunursa
        };
        
        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"User creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
        
        // User-ı yenidən yükləyək ki Id-si düzgün olsun
        var createdUser = await userManager.FindByNameAsync(username) ?? throw new InvalidOperationException("User was not found after creation");
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new Role() { Name = "Admin" });
        }
        
        var roleResult = await userManager.AddToRoleAsync(createdUser, "Admin");
        if (!roleResult.Succeeded)
        {
            throw new InvalidOperationException($"Adding user to role failed: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
        }
        
        return createdUser;
    }
    public async Task<User> CheckPasswordAsync(User user, string password)
    {
        return await userManager.CheckPasswordAsync(user, password)
            ? user
            : throw new UnauthorizedAccessException();
    }
}