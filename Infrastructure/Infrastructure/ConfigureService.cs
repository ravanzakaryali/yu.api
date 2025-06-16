namespace Yu.Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWorkService, UnitOfWorkService>();
        services.AddStorage<FireBaseStorage>();
        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        return services;
    }

    static void AddStorage<T>(this IServiceCollection services) where T : StorageHelper, IStorage
    {
        services.AddScoped<IStorage, T>();
    }
}