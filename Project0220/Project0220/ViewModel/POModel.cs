using Project0220.myModels;

namespace Project0220.ViewModel
{
    public class POModel { 
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? TotalAmount { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Carrier { get; set; }

    public DateOnly? ShippingDate { get; set; }

    public string? ShippingAddress { get; set; }

    public virtual Customer? Customer { get; set; }

    public string? Consignee { get; set; }

    public string? ContactPhone { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public int? SupplierId { get; set; }

    public int? CategoryId { get; set; }

    public int? UnitPrice { get; set; }

    public int? UnitInStock { get; set; }

    public string Image1 { get; set; } = null!;

    public string? Image2 { get; set; }

    public string? Image3 { get; set; }

    public string Color1 { get; set; } = null!;

    public string? Color2 { get; set; }

    public string? Color3 { get; set; }

    public string? Color4 { get; set; }

    public string? Length { get; set; }

    public string? Width { get; set; }

    public string? Height { get; set; }

    public string? SpecialZoneType { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Supplier? Supplier { get; set; }

    public List<Order> Orders { get; set; }
    public List<Product> Products { get; set; }
}
}
