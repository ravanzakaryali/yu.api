
namespace Yu.Persistence.Configurations;

public class DeleteOrderConfiguration : IEntityTypeConfiguration<DeleteOrder>
{
    public void Configure(EntityTypeBuilder<DeleteOrder> builder)
    {
        builder.ConfigureAuditableBaseEntity();
    }
}