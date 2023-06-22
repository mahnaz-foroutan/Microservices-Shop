using Ordering.Domain.Common;
using Ordering.Domain.Entities.Identity;
using System;
using System.Collections.Generic;

namespace Ordering.Domain.Entities
{
    public class Order : EntityBase
    {
        public Order()
        {
        }

        public Order(IReadOnlyList<OrderItem> orderItems, string buyerEmail, 
            Address shipToAddress, DeliveryMethod deliveryMethod, 
            decimal subtotal, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public Address ShipToAddress { get; set; }
        public DeliveryMethod? DeliveryMethod { get; set; }
        //private readonly List<OrderItem> _orderItems;
        //public IReadOnlyList<OrderItem> OrderItems => _orderItems;
        

        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string? PaymentIntentId { get; set; }

        //public decimal GetTotal()
        //{
        //    return Subtotal + DeliveryMethod.Price;
        //}

    }
}