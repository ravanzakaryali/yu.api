
namespace Yu.Persistence.Configurations;

public class OrderReasonConfiguration : IEntityTypeConfiguration<OrderReason>
{
    public void Configure(EntityTypeBuilder<OrderReason> builder)
    {
        builder.ConfigureAuditableBaseEntity();
    }
}