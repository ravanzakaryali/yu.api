namespace Yu.Domain.Entities;

public class Member : User
{
    public Member()
    {
        Addresses = new HashSet<Address>();
    }
    public ICollection<Address> Addresses { get; set; }
}