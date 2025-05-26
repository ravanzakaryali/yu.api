namespace Yu.Infrastructure.Concretes;

public class IdentityService(UserManager<User> userManager, RoleManager<Role> roleManager) : IIdentityService
{
    public async Task<Member> RegisterAsync(RegisterDto register)
    {
        Member newUser = new()
        {
            Name = register.Name,
            Surname = register.Surname,
            PhoneNumber = register.PhoneNumber,
            UserName = register.PhoneNumber,
            ConfirmCode = register.ConfirmCode,
            Email = null, // Assuming email is not required for registration
        };

        IdentityResult identityResultCreate = await userManager.CreateAsync(newUser);
        if (!identityResultCreate.Succeeded) throw new IdentityException("Create user exception");

        if (await roleManager.FindByNameAsync("Member") is null)
        {
            IdentityResult resultRole = await roleManager.CreateAsync(new Role()
            {
                Name = "Member"
            });
            if (!resultRole.Succeeded) throw new IdentityException("Create role exception");
        }
        IdentityResult result = await userManager.AddToRoleAsync(newUser, "Member");

        if (!result.Succeeded) throw new IdentityException("Register exception");

        return newUser!;
    }
}