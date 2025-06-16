namespace Yu.Persistence.Configurations;

public class OrderClothingItemConfiguration : IEntityTypeConfiguration<OrderClothingItem>
{
    public void Configure(EntityTypeBuilder<OrderClothingItem> builder)
    {
        builder.ConfigureAuditableBaseEntity();
    }
}