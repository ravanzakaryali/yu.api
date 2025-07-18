using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yu.Domain.Entities;

namespace Yu.Persistence.Configurations;

public class PromoCodeConfiguration : IEntityTypeConfiguration<PromoCode>
{
    public void Configure(EntityTypeBuilder<PromoCode> builder)
    {
        builder.ConfigureAuditableBaseEntity();

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Type)
            .IsRequired();

        builder.Property(p => p.Total)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.MinumumAmount)
            .HasPrecision(18, 2);

        builder.Property(p => p.StartDate)
            .IsRequired();

        builder.Property(p => p.EndDate);

        builder.Property(p => p.MaxUsageCount);

        builder.HasIndex(p => p.Code)
            .IsUnique();

        builder.HasMany(p => p.Orders)
            .WithOne(o => o.PromoCode)
            .HasForeignKey(o => o.PromoCodeId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
} 