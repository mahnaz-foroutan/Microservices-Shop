using MediatR;
using Ordering.Application.Features.User.Dtos;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommand : IRequest<int>
    { 

        public string? BuyerEmail { get; set; }
        
        public int? DelieveryMethod { get; set; }
        public string? BasketId { get; set; }
        public List<CheckoutOrderBasketItems> Items { get; set; } = new List<CheckoutOrderBasketItems>();
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal Subtotal { get; set; }
        public int? CouponTotal { get; set; }

        public AddressDto? ShipToAddress { get; set; }
      
    }
}
