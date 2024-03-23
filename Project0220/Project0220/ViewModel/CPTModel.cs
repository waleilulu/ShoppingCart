using Project0220.myModels;

namespace Project0220.ViewModel
{
    public class CPTModel
    {
        public List<Product>? Products { get; set; }
        public List<Customer>? Customers { get; set; }

        public List<TrackList>? TrackLists { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }
}
