using Microsoft.EntityFrameworkCore;

namespace Project0220.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }

    }
}
