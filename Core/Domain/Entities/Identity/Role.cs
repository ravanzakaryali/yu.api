using Microsoft.AspNetCore.Identity;

namespace Yu.Domain.Entities;

public class Role : IdentityRole<string>, IBaseEntity
{
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
}