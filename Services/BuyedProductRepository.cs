using Microsoft.EntityFrameworkCore;
using OnlineShopAPIFull.Data;
using OnlineShopAPIFull.Models;

namespace OnlineShopAPIFull.Services
{
    public class BuyedProductRepository : IBuyedProductRepository
    {
        private readonly AppDbContext _context;
        public BuyedProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateProduct(BuyedProduct product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var date = DateTime.UtcNow;
            product.buyedDate = date;
            await _context.BuyedProducts.AddAsync(product);
        }

        public async Task DeleteProduct(BuyedProduct product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            _context.BuyedProducts.Remove(product);
        }

        public async Task<BuyedProduct> Get(int id)
        {
            return await _context.BuyedProducts.FirstOrDefaultAsync(product => product.Id == id);
        }

        public async Task<IEnumerable<BuyedProduct>> GetAll()
        {
            return await _context.BuyedProducts.ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
