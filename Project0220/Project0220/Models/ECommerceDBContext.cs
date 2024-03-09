using Microsoft.EntityFrameworkCore;

namespace Project0220.Models
{
    public class ECommerceDBContext : DbContext
    {
        public ECommerceDBContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }

    }
}
