namespace Yu.Application.Abstractions;

public interface IYuDbContext
{
    DbSet<User> Users { get; }
    DbSet<Member> Members { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}