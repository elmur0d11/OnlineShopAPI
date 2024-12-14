using OnlineShopAPIFull.Models;

namespace OnlineShopAPIFull.Services
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll();

        Task<Product> Get(int id);

        Task CreateProduct(Product product);

        Task UpdateProduct(Product product);

        Task DeleteProduct(Product product);

        public Task<bool> SaveChangesAsync();
    }
}
