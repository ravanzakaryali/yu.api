namespace Yu.Application.Abstractions;


public interface IUserService
{
    Task<User> FindByIdAsync(string id);
    Task<User> AddAdminAsync(string fullName, string username, string password);
    Task<User> CheckPasswordAsync(User user, string password);
}