using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using OnlineShopAPIFull.Controllers;
using OnlineShopAPIFull.Dtos;
using OnlineShopAPIFull.Models;
using OnlineShopAPIFull.Services;
using OnlineShopAPIFull.Services.Caching;

namespace OnlineShopAPI.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly IBuyedProductRepository _buyedProductRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public UserControllerTests()
        {
            _productRepository = A.Fake<IProductRepository>();
            _buyedProductRepository = A.Fake<IBuyedProductRepository>();
            _mapper = A.Fake<IMapper>();
            _cacheService = A.Fake<ICacheService>();
        }

        #region UserController_GetProducts_ReturnOK
        [Fact]
        public async Task UserController_GetProducts_ReturnOK()
        {
            //Arrange
            var products = A.Fake<ICollection<Product>>();
            var productList = A.Fake<IEnumerable<ProductReadDto>>();

            A.CallTo(() => _productRepository.GetAll()).Returns(products);
            A.CallTo(() => _mapper.Map<IEnumerable<ProductReadDto>>(products)).Returns(productList);

            var controller = new UserController(_cacheService, _mapper, _productRepository, _buyedProductRepository);

            //Act
            var result = await controller.GetProducts();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }
        #endregion

        #region UserController_GetProduct_ReturnsOk
        [Fact]
        public async Task UserController_GetProduct_ReturnsOk()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Test Product", Description = "Test Description", Balance = 10, Price = 200 };
            var productDto = new ProductReadDto { Id = productId, Name = "Test Product", Description = "Test Description", Balance = 10, Price = 200 };

            A.CallTo(() => _productRepository.Get(productId)).Returns(product);

            var controller = new UserController(_cacheService, _mapper, _productRepository, _buyedProductRepository);

            // Act
            var result = await controller.GetProductById(productId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

        }
        #endregion

        #region UserController_PostBuyedProduct_ReturnsCreated
        [Fact]
        public async Task UserController_PostBuyedProduct_ReturnsCreated()
        {
            // Arrange
            var buyedProductCreatedDto = A.Fake<BuyedProductCreatedDto>();
            var buyedProduct = A.Fake<BuyedProduct>();
            var buyedProductReadDto = A.Fake<BuyedProductReadDto>();

            A.CallTo(() => _mapper.Map<BuyedProduct>(buyedProductCreatedDto)).Returns(buyedProduct);
            A.CallTo(() => _buyedProductRepository.CreateProduct(buyedProduct)).Returns(Task.CompletedTask);
            A.CallTo(() => _mapper.Map<BuyedProductReadDto>(buyedProduct)).Returns(buyedProductReadDto);

            var controller = new UserController(_cacheService, _mapper, _productRepository, _buyedProductRepository);

            //Act
            var result = await controller.CreateProduct(buyedProductCreatedDto);

            //Assert
            var createdResult = result.Should().BeOfType<CreatedResult>().Subject;
            result.Should().NotBeNull();
        }
        #endregion

        #region UserController_GetSelledProducts_ReturnOK
        [Fact]
        public async Task UserController_GetSelledProducts_ReturnOK()
        {
            //Arrange
            var buyedProducts = A.Fake<ICollection<BuyedProduct>>();
            var buyedProductList = A.Fake<IEnumerable<BuyedProductReadDto>>();

            A.CallTo(() => _buyedProductRepository.GetAll()).Returns(buyedProducts);
            A.CallTo(() => _mapper.Map<IEnumerable<BuyedProductReadDto>>(buyedProducts)).Returns(buyedProductList);

            var controller = new UserController(_cacheService, _mapper, _productRepository, _buyedProductRepository);

            //Act
            var result = await controller.GetSelledProducts();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }
        #endregion

    }
}
