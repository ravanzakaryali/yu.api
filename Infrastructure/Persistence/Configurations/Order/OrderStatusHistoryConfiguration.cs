namespace Yu.Persistence.Configurations;

public class OrderStatusHistoryConfiguration : IEntityTypeConfiguration<OrderStatusHistory>
{
    public void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
    {
        builder.ConfigureAuditableBaseEntity();

        builder.Property(oh => oh.SubStatus)
            .HasConversion(new EnumToStringConverter<Status>());

        builder.Property(oh => oh.OrderStatus)
            .HasConversion(new EnumToStringConverter<OrderStatus>());
    }
}