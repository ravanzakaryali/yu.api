namespace Yu.Application.Abstractions;


public interface IUserService
{
    Task<User> FindById(string id);
}