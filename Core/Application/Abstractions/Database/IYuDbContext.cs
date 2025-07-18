namespace Yu.Application.Abstractions;

public interface IYuDbContext
{
    DbSet<User> Users { get; }
    DbSet<Member> Members { get; }
    DbSet<Yu.Domain.Entities.File> Files { get; }
    DbSet<OrderImage> OrderImages { get; }
    DbSet<Service> Services { get; }
    DbSet<Price> Prices { get; }
    DbSet<ClothingItem> ClothingItems { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderService> OrderServices { get; }
    DbSet<Address> Addresses { get; }
    DbSet<CancelOrderReason> CancelOrderReasons { get; }
    DbSet<OrderStatusHistory> OrderStatusHistories { get; }
    DbSet<CancelOrder> CancelOrders { get; }
    DbSet<PickupDateSetting> PickupDateSettings { get; }
    DbSet<PromoCode> PromoCodes { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}