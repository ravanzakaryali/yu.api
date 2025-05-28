namespace Yu.Application.Abstractions;

public interface IYuDbContext
{
    DbSet<User> Users { get; }
    DbSet<Member> Members { get; }
    DbSet<Yu.Domain.Entities.File> Files { get; }
    DbSet<Service> Services { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}