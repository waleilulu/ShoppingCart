namespace Project0220.myModels
{
    public class CartOrderModel
    {
        public Order Order { get; set; }
        public List<CartItem> CartItem { get; set; }
        public Customer Customer { get; set; }
        public List<Product> Products { get; set; }

    }
}
