using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShopAPIFull.Services.Caching;
using OnlineShopAPIFull.Services;
using OnlineShopAPIFull.Dtos;
using OnlineShopAPIFull.Models;
using Serilog;
using Microsoft.AspNetCore.Http.HttpResults;

namespace OnlineShopAPIFull.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region DI
        private readonly IProductRepository _productRepository;
        private readonly IBuyedProductRepository _buyedProductRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public UserController(
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

        #region GetProducts
        [HttpGet("Products")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetAll();

                return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(products));
            }
            catch (Exception ex)
            {
                Log.Error($"Error-From-User-ErrorWith{ex}");
                return NotFound($"{ex}");
            }
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
                Log.Error($"Error-From-User-ErrorWith{ex}");
                return NotFound(ex);
            }
        }
        #endregion

        #region PostBuy
        [HttpPost("Buy")]
        public async Task<IActionResult> CreateProduct(BuyedProductCreatedDto buyedProductCreatedDto)
        {
            var productModel = _mapper.Map<BuyedProduct>(buyedProductCreatedDto);

            await _buyedProductRepository.CreateProduct(productModel);
            await _buyedProductRepository.SaveChangesAsync();

            var productReadDto = _mapper.Map<BuyedProductReadDto>(productModel);

            return Created("", productModel);

        }
        #endregion

        #region GetBuyedProducts
        [HttpGet("BuyedProducts")]
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
