namespace Yu.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");
        builder.Property(u => u.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(u => u.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(u => u.CreatedDate)
            .HasDefaultValueSql("getutcdate()")
            .IsRequired();

        builder.Property(u => u.UpdatedDate)
            .HasDefaultValueSql("getutcdate()")
            .IsRequired();

        builder.Property(u => u.Email)
            .HasDefaultValue("")
            .IsRequired(false);

        builder.HasQueryFilter(u => !u.IsDeleted && u.IsActive);

        builder.ToTable("Users", Schemas.Identity);
    }
}