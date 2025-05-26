namespace Yu.Application.Abstractions;

public interface ITokenService
{
    string GenerateVerificationCode(int length = 6);

    Task<TokenDto> GenerateTokenAsync<T>(T user) where T : User;

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

}