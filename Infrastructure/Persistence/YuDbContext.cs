
namespace Yu.Persistence;

public class YuDbContext(
        DbContextOptions<YuDbContext> options,
        EntitySaveChangesInterceptor entitySaveChangesInterceptor,
        OrderInterceptor orderInterceptor) : IdentityDbContext<User, Role, string>(options), IYuDbContext
{
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Domain.Entities.File> Files => Set<Domain.Entities.File>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Price> Prices => Set<Price>();
    public DbSet<ClothingItem> ClothingItems => Set<ClothingItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderService> OrderServices => Set<OrderService>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<CancelOrderReason> CancelOrderReasons => Set<CancelOrderReason>();
    public DbSet<OrderStatusHistory> OrderStatusHistories => Set<OrderStatusHistory>();
    public DbSet<CancelOrder> CancelOrders => Set<CancelOrder>();
    public DbSet<OrderImage> OrderImages => Set<OrderImage>();
    public DbSet<PickupDateSetting> PickupDateSettings => Set<PickupDateSetting>();
    public DbSet<PromoCode> PromoCodes => Set<PromoCode>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors([entitySaveChangesInterceptor, orderInterceptor]);
        base.OnConfiguring(optionsBuilder);
    }
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