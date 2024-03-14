using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project0220.myModels;

public partial class Order
{
	public int OrderId { get; set; }

	public int? CustomerId { get; set; }

<<<<<<< HEAD
	public DateTime? OrderDate { get; set; }
=======
    //下方設定日期: 年/月/日 沒有幾點幾分
    [DataType(DataType.Date)]
    public DateTime? OrderDate { get; set; }
>>>>>>> 0313

	public int? TotalAmount { get; set; }

	public string? PaymentMethod { get; set; }

	public string? Carrier { get; set; }

<<<<<<< HEAD
	public DateOnly? ShippingDate { get; set; }
=======
    //下方設定日期: 年/月/日 沒有幾點幾分
    [DataType(DataType.Date)]
    public DateOnly? ShippingDate { get; set; }
>>>>>>> 0313

	public string? PostalCode { get; set; }

	public string? ShippingAddress { get; set; }

	public virtual Customer? Customer { get; set; }

	public string? Consignee { get; set; }

<<<<<<< HEAD
	public string? ContactPhone { get; set; }

	public string? Status { get; set; }

	public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
=======
    public string? Status { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
>>>>>>> 0313
}
