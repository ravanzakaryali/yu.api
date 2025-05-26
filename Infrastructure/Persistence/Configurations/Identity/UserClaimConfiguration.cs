namespace Yu.Persistence.Configurations;

public class UserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
    {

        builder.ToTable("UserClaims", Schemas.Identity);
    }
}