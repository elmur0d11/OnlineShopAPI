using Microsoft.EntityFrameworkCore;
using OnlineShopAPIFull.Data;
using OnlineShopAPIFull.Models;
using OnlineShopAPIFull.Services.Caching;

namespace OnlineShopAPIFull.Services.Hangfire
{
    public class ServiceManagement : IServiceManagement
    {
        private readonly AppDbContext _context;
        private readonly ICacheService _cacheService;

        public ServiceManagement(AppDbContext context, ICacheService cacheService)
        {
            _cacheService = cacheService;
            _context = context;
        }

        public async Task GetCacheData()
        {
            var products = _cacheService.GetData<IEnumerable<Product>>("products");
            var buyedProducts = _cacheService.GetData<IEnumerable<BuyedProduct>>("buyedProducts");

            if (products != null && buyedProducts != null)
            {
                Console.WriteLine("You have the data in cache..");
            }

            Console.WriteLine("Cache is clear!");
        }
    }
}