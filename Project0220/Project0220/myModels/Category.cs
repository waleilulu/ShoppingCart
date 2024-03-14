using System;
using System.Collections.Generic;


namespace Project0220.myModels;

public class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

	//public DateTime CreatedAt { get; set; }

	public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
