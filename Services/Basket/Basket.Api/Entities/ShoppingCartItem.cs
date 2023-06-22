namespace Basket.Api.Entities
{
    public class ShoppingCartItem
    {

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string ProductId { get; set; }

        public string ProductName { get; set; }
        public string? PictureUrl { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public int? DiscountAmount { get;  set; }
    }
}
