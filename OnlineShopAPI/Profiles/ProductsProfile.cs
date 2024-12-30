using AutoMapper;
using OnlineShopAPIFull.Dtos;
using OnlineShopAPIFull.Models;

namespace OnlineShopAPIFull.Profiles
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            CreateMap<ProductCreatedDto, Product>();
            CreateMap<Product, ProductReadDto>();
        }
    }
}
