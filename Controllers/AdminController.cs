using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShopAPIFull.Dtos;
using OnlineShopAPIFull.Models;
using OnlineShopAPIFull.Services;
using OnlineShopAPIFull.Services.Caching;
using OnlineShopAPIFull.Services.Hangfire;
using Serilog;

namespace OnlineShopAPIFull.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        #region DI
        private readonly IProductRepository _productRepository;
        private readonly IBuyedProductRepository _buyedProductRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public AdminController(
            ICacheService cacheService,
            IMapper mapper,
            IProductRepository productRepository,
            IBuyedProductRepository buyedProductRepository)
        {
            _cacheService = cacheService;
            _mapper = mapper;
            _productRepository = productRepository;
            _buyedProductRepository = buyedProductRepository;
        }
        #endregion

        #region GetAll
        [HttpGet("Products")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var cacheNames = _cacheService.GetData<IEnumerable<Product>>("products");

                if (cacheNames != null)
                {
                    Console.WriteLine("CACHE HIT!");
                    return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(cacheNames));
                }


                Console.WriteLine("CACHE MISS!");
                var products = await _productRepository.GetAll();

                var expiryTime = DateTime.UtcNow.AddMinutes(5);

                _cacheService.SetData("products", products, expiryTime);

                return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(products));

            }
            catch (Exception ex)
            {
                return NotFound($"Server Chichvordi! >> {ex}");
            }
        }
        #endregion

        #region PostProduct
        [HttpPost("Products")]
        public async Task<IActionResult> CreateProduct(ProductCreatedDto productCreatedDto)
        {
            var productModel = _mapper.Map<Product>(productCreatedDto);

            await _productRepository.CreateProduct(productModel);
            await _productRepository.SaveChangesAsync();

            var productReadDto = _mapper.Map<ProductReadDto>(productModel);

            BackgroundJob.Enqueue<IServiceManagement>(
                service => service.RefreshCacheAsync());

            return Created("", productModel);
            
        }
        #endregion

        #region GetById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetName(int id)
        {
            try
            {
                var cacheKey = $"products_{id}";

                var cachedProduct = _cacheService.GetData<Product>(cacheKey);
                if (cachedProduct != null)
                {
                    Console.WriteLine("Cache HIT!");
                    return Ok(_mapper.Map<ProductReadDto>(cachedProduct));
                }

                Console.WriteLine("Cache MISS!");
                var product = await _productRepository.Get(id);
                if (product == null)
                    return NotFound();

                var expiryTime = DateTimeOffset.Now.AddMinutes(5);
                _cacheService.SetData(cacheKey, product, expiryTime);

                return Ok(_mapper.Map<ProductReadDto>(product));
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
        #endregion

        #region DeleteProduct
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var productModelFromRepo = await _productRepository.Get(id);

            if (productModelFromRepo == null)
                return NotFound();

            var cacheKey = $"{id}";
            _cacheService.RemoveData(cacheKey);

            await _productRepository.DeleteProduct(productModelFromRepo);

            await _productRepository.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region GetSelledProduct
        [HttpGet("SelledProduct")]
        public async Task<IActionResult> GetSelledProducts()
        {
            try
            {
                var products = await _buyedProductRepository.GetAll();

                return Ok(_mapper.Map<IEnumerable<BuyedProductReadDto>>(products));
            }
            catch (Exception ex)
            {
                Log.Error($"Error-From-Admin-{ex}");
                return NotFound($"{ex}");
            }
        }
        #endregion
    }
}
