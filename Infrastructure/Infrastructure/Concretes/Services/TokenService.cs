namespace Yu.Infrastructure.Concretes;

public class TokenService(UserManager<User> userManager, IConfiguration configuration) : ITokenService
{
    public async Task<TokenDto> GenerateTokenAsync<T>(T user) where T : User
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? DateTime.Now.Ticks.ToString()),
        };
        IList<string> roles = await userManager.GetRolesAsync(user);

        if (roles != null)
        {
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        }
        DateTime expires = DateTime.UtcNow.AddHours(4).AddMonths(1);

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt:SecurityKey").Value!));

        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken securityToken = new(
            issuer: configuration.GetSection("Jwt:Issuer").Value,
            audience: configuration.GetSection("Jwt:Audience").Value,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        return new TokenDto()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(securityToken),
            Expires = expires,
        };
    }

    public string GenerateVerificationCode(int length = 6)
    {
        // const string chars = "0123456789";
        // var random = new Random();

        // return new string(Enumerable.Repeat(chars, length)
        //     .Select(s => s[random.Next(s.Length)]).ToArray());
        return "111111"; // For testing purposes, return a fixed code
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        TokenValidationParameters tokenValidationParameters = new()
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecurityKey"]!)),
            ValidAudience = configuration["Jwt:Audience"],
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidateLifetime = false
        };

        JwtSecurityTokenHandler tokenHandler = new();
        ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }
        return principal;
    }
}