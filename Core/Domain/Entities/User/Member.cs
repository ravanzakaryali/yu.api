namespace Yu.Domain.Entities;

public class Member : User
{
    public Member()
    {
        Addresses = new HashSet<Address>();
        Orders = new HashSet<Order>();
    }
    public ICollection<Address> Addresses { get; set; }
    public ICollection<Order> Orders { get; set; } = null!;
}