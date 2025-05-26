namespace Yu.Application.Abstractions;

public interface IRoleService
{
    Task<IList<string>> GetRolesByUser(User user);
}