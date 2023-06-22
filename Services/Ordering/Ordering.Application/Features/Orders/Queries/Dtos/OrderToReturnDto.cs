using AutoMapper;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Queries.Dtos
{
    public class OrderToReturnDto 
    {
        //public OrderToReturnDto()
        //{
        //    _orderItems = new List<OrderItemDto>();
        //}
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public Address ShipToAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal ShippingPrice { get; set; }
       // private readonly List<OrderItemDto> _orderItems;
       // public List<OrderItemDto> OrderItems => _orderItems;
        public IReadOnlyList<OrderItemDto> OrderItems { get; set; } //= new List<OrderItemDto>();
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
       

    }
}
