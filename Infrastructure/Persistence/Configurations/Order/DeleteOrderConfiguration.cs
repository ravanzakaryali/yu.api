
namespace Yu.Persistence.Configurations;

public class CancelOrderConfiguration : IEntityTypeConfiguration<CancelOrder>
{
    public void Configure(EntityTypeBuilder<CancelOrder> builder)
    {
        builder.ConfigureAuditableBaseEntity();
    }
}