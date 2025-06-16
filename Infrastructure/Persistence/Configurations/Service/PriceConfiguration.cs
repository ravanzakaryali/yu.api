namespace Yu.Persistence.Configurations;

public class PriceConfiguration : IEntityTypeConfiguration<Price>
{
    public void Configure(EntityTypeBuilder<Price> builder)
    {
        builder.ConfigureAuditableBaseEntity();

        builder.HasOne(p => p.Service)
            .WithOne(s => s.Price)
            .HasForeignKey<Price>(p => p.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}