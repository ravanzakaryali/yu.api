
namespace Yu.Persistence.Configurations;

public class ClothingItemConfiguration : IEntityTypeConfiguration<ClothingItem>
{
    public void Configure(EntityTypeBuilder<ClothingItem> builder)
    {
        builder.ConfigureAuditableBaseEntity();

        builder.HasOne(ci => ci.Price)
            .WithOne(p => p.ClothingItem)
            .HasForeignKey<ClothingItem>(ci => ci.PriceId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}