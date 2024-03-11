using System;
using System.Collections.Generic;

namespace Project0220.myModels;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string? CompanyName { get; set; }

    public string? ContactName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
