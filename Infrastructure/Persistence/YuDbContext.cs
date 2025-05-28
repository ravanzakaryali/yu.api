namespace Yu.Persistence;

public class YuDbContext(DbContextOptions<YuDbContext> options) : IdentityDbContext<User, Role, string>(options), IYuDbContext
{
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Domain.Entities.File> Files => Set<Domain.Entities.File>();
    public DbSet<Service> Services => Set<Service>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}