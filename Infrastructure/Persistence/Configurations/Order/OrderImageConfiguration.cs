
namespace Yu.Persistence.Configurations;

public class OrderImageConfiguration : IEntityTypeConfiguration<OrderImage>
{
    public void Configure(EntityTypeBuilder<OrderImage> builder)
    {
        builder.ConfigureAuditableBaseEntity();
    }
}