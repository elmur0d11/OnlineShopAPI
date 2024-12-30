using AutoMapper;
using OnlineShopAPIFull.Services.Caching;
using OnlineShopAPIFull.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using OnlineShopAPIFull.Controllers;
using OnlineShopAPIFull.Dtos;
using OnlineShopAPIFull.Models;
using FluentAssertions;

namespace OnlineShopAPI.Tests.Controllers
{
    public class AdminControllerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly IBuyedProductRepository _buyedProductRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public AdminControllerTests()
        {
            _productRepository = A.Fake<IProductRepository>();
            _buyedProductRepository = A.Fake<IBuyedProductRepository>();
            _mapper = A.Fake<IMapper>();
            _cacheService = A.Fake<ICacheService>();
        }

        #region AdminController_GetProducts_ReturnsOk
        [Fact]
        public async Task AdminController_GetProducts_ReturnsOk()
        {
            //Arrange
            var products = A.Fake<ICollection<Product>>();
            var productList = A.Fake<IEnumerable<ProductReadDto>>();

            A.CallTo(() => _productRepository.GetAll()).Returns(products);
            A.CallTo(() => _mapper.Map<IEnumerable<ProductReadDto>>(products)).Returns(productList);

            var controller = new AdminController(_cacheService, _mapper, _productRepository, _buyedProductRepository);

            //Act
            var result = await controller.GetProducts();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }
        #endregion

        #region AdminController_CreateProduct_ReturnsCreated
        [Fact]
        public async Task AdminController_CreateProduct_ReturnsCreated()
        {
            // Arrange
            var productCreatedDto = A.Fake<ProductCreatedDto>();
            var product = A.Fake<Product>();
            var productReadDto = A.Fake<ProductReadDto>();

            A.CallTo(() => _mapper.Map<Product>(productCreatedDto)).Returns(product);
            A.CallTo(() => _productRepository.CreateProduct(product)).Returns(Task.CompletedTask);
            A.CallTo(() => _mapper.Map<ProductReadDto>(product)).Returns(productReadDto);
            A.CallTo(() => _productRepository.SaveChangesAsync()).Returns(Task.FromResult(true));
            A.CallTo(() => _cacheService.RemoveData("products")).Returns(Task.FromResult(true));
            

            var controller = new AdminController(_cacheService, _mapper, _productRepository, _buyedProductRepository);

            //Act
            var result = await controller.CreateProduct(productCreatedDto);

            //Assert
            var createdResult = result.Should().BeOfType<CreatedResult>().Subject;
            result.Should().NotBeNull();
        }
        #endregion

        #region AdminController_GetProductById_ReturnsOk
        [Fact]
        public async Task AdminController_GetProductById_ReturnsOk()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Test Product", Description = "Test Description", Balance = 10, Price = 200 };
            var productDto = new ProductReadDto { Id = productId, Name = "Test Product", Description = "Test Description", Balance = 10, Price = 200 };

            A.CallTo(() => _productRepository.Get(productId)).Returns(product);

            var controller = new AdminController(_cacheService, _mapper, _productRepository, _buyedProductRepository);

            // Act
            var result = await controller.GetProductById(productId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
        #endregion

        #region AdminController_Delete_ReturnsNoContent
        [Fact]
        public async Task AdminController_Delete_ReturnsNoContent()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Test Product", Description = "Test Description", Balance = 10, Price = 200 };

            A.CallTo(() => _productRepository.Get(productId))
                .Returns(Task.FromResult(product));

            A.CallTo(() => _cacheService.RemoveData($"{productId}")).Returns(Task.FromResult(true));

            A.CallTo(() => _productRepository.DeleteProduct(product))
                .Returns(Task.CompletedTask);

            A.CallTo(() => _productRepository.SaveChangesAsync())
                .Returns(Task.FromResult(true));

            var controller = new AdminController(_cacheService, _mapper, _productRepository, _buyedProductRepository);

            // Act
            var result = await controller.DeleteProduct(productId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }
        #endregion

        #region AdminController_GetSelledProducts_ReturnOK
        [Fact]
        public async Task AdminController_GetSelledProducts_ReturnOK()
        {
            //Arrange
            var buyedProducts = A.Fake<ICollection<BuyedProduct>>();
            var buyedProductList = A.Fake<IEnumerable<BuyedProductReadDto>>();

            A.CallTo(() => _buyedProductRepository.GetAll()).Returns(buyedProducts);
            A.CallTo(() => _mapper.Map<IEnumerable<BuyedProductReadDto>>(buyedProducts)).Returns(buyedProductList);

            var controller = new AdminController(_cacheService, _mapper, _productRepository, _buyedProductRepository);

            //Act
            var result = await controller.GetSelledProducts();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }
        #endregion
        
    }
}
