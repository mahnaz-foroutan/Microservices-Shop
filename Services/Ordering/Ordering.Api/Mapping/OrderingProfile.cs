using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.User.Dtos;

namespace Ordering.Api.Mapping
{
    public class OrderingProfile : Profile
    {
        public OrderingProfile()
        {
            CreateMap<CheckoutOrderCommand, BasketCheckoutEvent>()
                .ForMember(d => d.DelieveryMethod, o => o.MapFrom(s => s.DelieveryMethod))
                .ForMember(d => d.Items, o => o.MapFrom(s => s.Items))
                .ReverseMap();
            CreateMap<CheckoutOrderBasketItems, BasketCheckoutBasketItems>()
               .ReverseMap();
            CreateMap<Ordering.Domain.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<Ordering.Domain.Entities.Address, AddressDto>().ReverseMap();
            CreateMap<Ordering.Domain.Entities.Identity.Address, Ordering.Domain.Entities.Address>().ReverseMap();

        }
    }
}
