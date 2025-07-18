
namespace Yu.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ConfigureAuditableBaseEntity();

        builder
            .HasOne(o => o.Member)
            .WithMany(m => m.Orders)
            .HasForeignKey(o => o.MemberId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(o => o.PickupDateSetting)
            .WithMany(p => p.Orders)
            .HasForeignKey(o => o.PickupDateSettingId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
        
    }
}