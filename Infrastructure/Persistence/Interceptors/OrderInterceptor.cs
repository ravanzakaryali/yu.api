namespace Yu.Persistence.Interceptors;

public class OrderInterceptor() : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        CreateOrder(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    public static void CreateOrder(DbContext? context)
    {
        if (context == null) return;

        foreach (EntityEntry<Order> entity in context.ChangeTracker.Entries<Order>())
        {
            if (entity.State == EntityState.Added)
            {
                entity.Entity.OrderNumber = entity.Entity.MemberId + "-" + DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            }
        }
    }
}