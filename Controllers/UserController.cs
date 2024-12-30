using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineShopAPIFull.Services.Caching;
using OnlineShopAPIFull.Services;
using OnlineShopAPIFull.Dtos;
using OnlineShopAPIFull.Models;
using Serilog;

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
                var cacheNames = _cacheService.GetData<IEnumerable<Product>>("products");

                if (cacheNames != null)
                    return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(cacheNames));

                var products = await _productRepository.GetAll();

                var expiryTime = DateTime.UtcNow.AddMinutes(5);

                _cacheService.SetData("products", products, expiryTime);

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
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var cacheKey = $"products_{id}";

                var cachedProduct = _cacheService.GetData<Product>(cacheKey);
                if (cachedProduct != null)
                    return Ok(_mapper.Map<ProductReadDto>(cachedProduct));

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

            _cacheService.RemoveData("buyedProducts");

            return Created("", productModel);

        }
        #endregion

        #region GetBuyedProducts
        [HttpGet("BuyedProducts")]
        public async Task<IActionResult> GetSelledProducts()
        {
            try
            {
                var cacheNames = _cacheService.GetData<IEnumerable<BuyedProduct>>("buyedProducts");

                if (cacheNames != null)
                    return Ok(_mapper.Map<IEnumerable<BuyedProductReadDto>>(cacheNames));

                var products = await _buyedProductRepository.GetAll();

                var expiryTime = DateTime.UtcNow.AddMinutes(5);

                _cacheService.SetData("buyedProducts", products, expiryTime);

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
