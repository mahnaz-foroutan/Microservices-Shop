using AutoMapper;
using Basket.Api.Entities;
using EventBus.Messages.Events;

namespace Basket.Api.Mapper
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<ShoppingCart, BasketCheckoutEvent>()
               .ForMember(d => d.BasketId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.DelieveryMethod, o => o.MapFrom(s => s.DeliveryMethodId))
                .ForMember(d => d.Items, o => o.MapFrom(s => s.Items))
                .ReverseMap();
            CreateMap<ShoppingCartItem, BasketCheckoutBasketItems>().ReverseMap();
        }
    }
}
