namespace Yu.Persistence.Interceptors;

public class EntitySaveChangesInterceptor(ICurrentUserService currentUserService) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (EntityEntry<BaseAuditableEntity> entity in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entity.State == EntityState.Added)
            {
                entity.Entity.CreatedDate = DateTime.UtcNow.AddHours(4);
                entity.Entity.CreatedBy = currentUserService.UserId ?? "System";
            }
            else if (entity.State == EntityState.Modified)
            {
                entity.Entity.UpdatedDate = DateTime.UtcNow.AddHours(4);
                entity.Entity.UpdatedBy = currentUserService.UserId ?? "System";
            }
        }
    }
}
