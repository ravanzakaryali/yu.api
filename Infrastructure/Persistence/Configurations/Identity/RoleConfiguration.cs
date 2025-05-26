namespace Yu.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");
        builder.Property(r => r.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(r => r.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.ToTable("Roles", Schemas.Identity);
    }
}