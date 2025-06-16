namespace Yu.Application.Abstractions;

public interface IYuDbContext
{
    DbSet<User> Users { get; }
    DbSet<Member> Members { get; }
    DbSet<Yu.Domain.Entities.File> Files { get; }
    DbSet<Service> Services { get; }
    DbSet<Price> Prices { get; }
    DbSet<ClothingItem> ClothingItems { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderService> OrderServices { get; }
    DbSet<Address> Addresses { get; }
    DbSet<OrderReason> OrderReasons { get; }
    DbSet<OrderStatusHistory> OrderStatusHistories { get; }
    DbSet<DeleteOrder> DeleteOrders { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}