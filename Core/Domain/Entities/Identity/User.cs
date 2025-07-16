namespace Yu.Domain.Entities;

public class User : IdentityUser<string>, IBaseEntity
{
    public User()
    {
        Id = Guid.NewGuid().ToString();
        SecurityStamp = Guid.NewGuid().ToString();
        CreatedDate = DateTime.UtcNow;
        UpdatedDate = DateTime.UtcNow;
    }

    public string FullName { get; set; } = null!;
    public string? ConfirmCode { get; set; }
    public override string? Email { get; set; }
    public DateTime? CreatedConfirmCodeDate { get; set; }
    public int ConfirmCodeCount { get; set; } = 0;
    public DateTime? LastLoginDate { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}