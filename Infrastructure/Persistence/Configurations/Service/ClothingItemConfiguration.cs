
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

        builder.HasMany(c => c.Services)
            .WithMany(s => s.ClothingItems)
            .UsingEntity<Dictionary<string, object>>("ClothingItemService",
                j => j
                    .HasOne<Service>()
                    .WithMany()
                    .HasForeignKey("ServicesId")
                    .OnDelete(DeleteBehavior.Restrict),
                j => j
                    .HasOne<ClothingItem>()
                    .WithMany()
                    .HasForeignKey("ClothingItemsId")
                    .OnDelete(DeleteBehavior.Restrict));

    }
}