namespace Yu.Application.Abstractions;

public interface IIdentityService
{
    Task<Member> RegisterAsync(RegisterDto register);
}