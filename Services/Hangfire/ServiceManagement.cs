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

        public async Task RefreshCacheAsync()
        {
            Console.WriteLine("Start refreshing cache...");

            try
            {
                _cacheService.RemoveData("products");

                var products = await _context.Products.ToListAsync();

                var cacheKey = "products";

                var expiryTime = DateTimeOffset.Now.AddMinutes(5);

                _cacheService.SetData(cacheKey, products, expiryTime);

                Console.WriteLine("Cache successfully refreshed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with refreshing cache: {ex.Message}");
            }
        }
    }
}