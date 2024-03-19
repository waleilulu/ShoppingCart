using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project0220.myModels
{
    public class CartItem
    {
        [Key]
        public int CartItemID { get; set; }

        public int CustomerID { get; set; }

        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public string SelectedColor { get; set; }

        // 導航屬性
        public virtual Customer Customer { get; set; }

        public virtual Product Product { get; set; }


    }
}
