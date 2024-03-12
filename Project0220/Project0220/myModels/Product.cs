using System;
using System.Collections.Generic;

namespace Project0220.myModels;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public int? SupplierId { get; set; }

    public int? CategoryId { get; set; }

    public int? UnitPrice { get; set; }

    public int? UnitInStock { get; set; }

    public string Image1 { get; set; } = null!;

    public string? Image2 { get; set; }

    public string? Image3 { get; set; }
    public string? Image4 { get; set; }

    public string? Image4 { get; set; }

    public string Color1 { get; set; } = null!;

    public string? Color2 { get; set; }

    public string? Length { get; set; }

    public string? Width { get; set; }

    public string? Height { get; set; }

    public string? Description { get; set; }

    public string? SpecialZoneType { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Supplier? Supplier { get; set; }
}
