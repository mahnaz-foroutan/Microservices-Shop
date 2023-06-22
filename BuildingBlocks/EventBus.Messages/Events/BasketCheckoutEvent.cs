namespace EventBus.Messages.Events
{
    public class BasketCheckoutEvent : IntegrationBaseEvent
    {
        public string BasketId { get; set; }
        public string? BuyerEmail { get; set; }

        public int? DelieveryMethod { get; set; }
        public List<BasketCheckoutBasketItems> Items { get; set; } = new List<BasketCheckoutBasketItems>();

        public decimal ShippingPrice { get; set; }
        public decimal Subtotal { get; set; }

        public int? CouponTotal { get; set; }
    }
}
