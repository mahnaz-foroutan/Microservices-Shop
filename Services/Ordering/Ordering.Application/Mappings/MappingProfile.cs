using AutoMapper;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.Dtos;
using Ordering.Application.Features.User.Dtos;
using Ordering.Domain.Entities;

namespace Ordering.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price))
            .ForMember(d => d.OrderItems, o => o.MapFrom(s => s.OrderItems))
            .ForMember(d => d.OrderDate, o => o.MapFrom(s => s.OrderDate.DateTime))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status));

            CreateMap<CheckoutOrderCommand, Order>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DelieveryMethod))
                .ForMember(d => d.OrderItems, o => o.MapFrom(s => s.Items))
                .ReverseMap();

            CreateMap<Order, CheckoutOrderCommand>().ReverseMap();
            CreateMap<Order, UpdateOrderCommand>().ReverseMap();
            CreateMap<AddressDto,Address>().ReverseMap();
            CreateMap<Ordering.Domain.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ProductItemId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.PictureUrl));
            CreateMap<OrderItem, CheckoutOrderBasketItems>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ProductItemId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.PictureUrl)).ReverseMap();
            CreateMap<DeliveryMethod, Task<DeliveryMethod>>().ReverseMap();

        }
    }
}