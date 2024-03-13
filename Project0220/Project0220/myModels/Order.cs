using System;
using System.Collections.Generic;

namespace Project0220.myModels;

public partial class Order
{
	public int OrderId { get; set; }

	public int? CustomerId { get; set; }

	public DateTime? OrderDate { get; set; }

	public int? TotalAmount { get; set; }

	public string? PaymentMethod { get; set; }

	public string? Carrier { get; set; }

	public DateOnly? ShippingDate { get; set; }

	public string? PostalCode { get; set; }

	public string? ShippingAddress { get; set; }

	public virtual Customer? Customer { get; set; }

	public string? Consignee { get; set; }

	public string? ContactPhone { get; set; }

	public string? Status { get; set; }

	public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
