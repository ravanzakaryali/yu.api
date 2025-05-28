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

        builder.ToTable("Services", Schemas.Default);
    }
}