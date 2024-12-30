using OnlineShopAPIFull.Models;

namespace OnlineShopAPIFull.Services
{
    public interface IBuyedProductRepository
    {
        Task<IEnumerable<BuyedProduct>> GetAll();

        Task<BuyedProduct> Get(int id);

        Task CreateProduct(BuyedProduct product);

        Task DeleteProduct(BuyedProduct product);

        public Task<bool> SaveChangesAsync();
    }
}
