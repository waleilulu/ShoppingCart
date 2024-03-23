using System;
using System.Collections.Generic;


namespace Project0220.myModels;

public class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public int? UnitPrice { get; set; }

    public decimal? Discount { get; set; }

    public decimal? Amount { get; set; }

    public string? SelectedColor { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }

	//public DateTime CreatedAt { get; set; }
}
