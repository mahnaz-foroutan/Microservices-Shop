using MediatR;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand :  IRequest
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }

        public int? DelieveryMethod { get; set; }
       // public string BasketId { get; set; }
        public List<CheckoutOrderBasketItems> Items { get; set; } = new List<CheckoutOrderBasketItems>();
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal Subtotal { get; set; }

        public Address? ShipToAddress { get; set; }
        // address

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
    }
}
