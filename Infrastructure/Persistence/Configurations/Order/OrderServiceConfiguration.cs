
namespace Yu.Persistence.Configurations;

public class OrderServiceConfiguration : IEntityTypeConfiguration<OrderService>
{
    public void Configure(EntityTypeBuilder<OrderService> builder)
    {
        builder.ConfigureAuditableBaseEntity();

        builder
            .HasMany(os => os.OrderClothingItems)
            .WithOne(o => o.OrderService)
            .HasForeignKey(os => os.OrderServiceId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}