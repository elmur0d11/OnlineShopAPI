using Microsoft.EntityFrameworkCore;
using OnlineShopAPIFull.Models;

namespace OnlineShopAPIFull.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //DB-Sets
        public DbSet<Product> Products { get; set; }
        public DbSet<BuyedProduct> BuyedProducts { get; set; }
    }
}
