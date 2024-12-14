using AutoMapper;
using OnlineShopAPIFull.Dtos;
using OnlineShopAPIFull.Models;

namespace OnlineShopAPIFull.Profiles
{
    public class BuyedProductProfile : Profile
    {
        public BuyedProductProfile()
        {
            CreateMap<BuyedProductCreatedDto, BuyedProduct>();
            CreateMap<BuyedProduct, BuyedProductReadDto>();
        }
    }
}
