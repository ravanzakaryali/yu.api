namespace Yu.Domain.Entities.Base;

public abstract class BaseEntity : IBaseEntity
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool IsActive { get; set; } = true;
}