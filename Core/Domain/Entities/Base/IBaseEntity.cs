namespace Yu.Domain.Entities.Base;

public interface IBaseEntity
{
    bool IsDeleted { get; set; }
    bool IsActive { get; set; }
}