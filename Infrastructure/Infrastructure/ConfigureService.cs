namespace Yu.Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWorkService, UnitOfWorkService>();
        return services;
    }
}