using Project0220.myModels;

namespace Project0220.ViewModel
{
    public class CPTModel
    {
        public List<Product>? Products { get; set; } // 用于其它目的，比如历史订单中的商品
        public List<Product>? TrackedProducts { get; set; } // 新增，专门用来存储追踪的商品
        public List<Customer>? Customers { get; set; }
        public List<TrackList>? TrackLists { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public List<Order> Orders { get; set; }
        public List<OrderWithDetails> OrdersWithDetails { get; set; } = new List<OrderWithDetails>();
    }

    // 代表單一訂單及其詳情的類
    public class OrderWithDetails
    {
        public Order Order { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
