namespace Yu.Persistence;

public static class ConfigureService
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<YuDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SqlServer"),
                b => b.MigrationsAssembly(typeof(YuDbContext).Assembly.FullName)));

        services.AddScoped<IYuDbContext>(provider => provider.GetRequiredService<YuDbContext>());

        services.AddIdentity<User, Role>(opt =>
        {
            opt.User.RequireUniqueEmail = false;
            opt.SignIn.RequireConfirmedPhoneNumber = true;
            opt.SignIn.RequireConfirmedEmail = false;
        })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<YuDbContext>();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(option =>
        {
            option.RequireHttpsMetadata = false;
            option.SaveToken = true;
            option.TokenValidationParameters = new TokenValidationParameters
            {
                ValidAudience = configuration.GetSection("Jwt:Audience").Value,
                ValidIssuer = configuration.GetSection("Jwt:Issuer").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt:SecurityKey").Value!)),
                ClockSkew = TimeSpan.Zero,
            };
        });

        return services;
    }
}