namespace Yu.Persistence.Configurations;

public class FileConfiguration : IEntityTypeConfiguration<Domain.Entities.File>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.File> builder)
    {
        builder.ConfigureAuditableBaseEntity();

        builder.ToTable("Files", Schemas.Default);
    }
}