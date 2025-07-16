
namespace Yu.Persistence.Configurations;

public class CancelOrderReasonConfiguration : IEntityTypeConfiguration<CancelOrderReason>
{
    public void Configure(EntityTypeBuilder<CancelOrderReason> builder)
    {
        builder.ConfigureAuditableBaseEntity();
    }
}