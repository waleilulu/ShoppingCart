using Microsoft.EntityFrameworkCore;

namespace Project0220.Models
{
    public class ManualECommerceDBContext : DbContext
    {
        public ManualECommerceDBContext(DbContextOptions <ManualECommerceDBContext>options) : base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }

    }
}
