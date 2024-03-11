using System;
using System.Collections.Generic;

namespace Project0220.myModels;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? CustomerName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? MobilePhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? AddressCity { get; set; }

    public string? AddressDist { get; set; }

    public string? Address { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
