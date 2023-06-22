namespace Ordering.Application.Features.Orders.Queries.Dtos
{
    public class OrderItemDto
    {
        public OrderItemDto()
        {
        }

        public OrderItemDto(int quantity, decimal price, string productId, string productName, string? pictureUrl)
        {
            Quantity = quantity;
            Price = price;
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string? PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}