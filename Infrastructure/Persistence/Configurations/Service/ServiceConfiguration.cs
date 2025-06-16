namespace Yu.Persistence.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ConfigureAuditableBaseEntity();

        builder.Property(s => s.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.SubTitle)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Desciption)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(s => s.ServiceType)
            .HasConversion(new EnumToStringConverter<ServiceType>())
            .HasDefaultValue(ServiceType.OnlyCount)
            .IsRequired();

        builder.ToTable("Services", Schemas.Default);
    }
}