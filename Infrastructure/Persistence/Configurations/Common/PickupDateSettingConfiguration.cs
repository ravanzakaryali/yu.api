namespace Yu.Persistence.Configurations;

public class PickupDateSettingConfiguration : IEntityTypeConfiguration<Domain.Entities.PickupDateSetting>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.PickupDateSetting> builder)
    {
        builder.ConfigureAuditableBaseEntity();

        builder.Property(p => p.DayOfWeek)
            .HasConversion(new EnumToStringConverter<DayOfWeek>())
            .IsRequired();
    }
}