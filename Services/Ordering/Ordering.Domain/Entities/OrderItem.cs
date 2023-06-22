using Ordering.Domain.Common;

namespace Ordering.Domain.Entities
{
    public class OrderItem : EntityBase
    {
        public OrderItem()
        {
        }

        public OrderItem(string productItemId ,string productName ,string pictureUrl , decimal price, int quantity,int? discountAmount)
        {
            ProductItemId = productItemId;
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
            Quantity = quantity;
            DiscountAmount = discountAmount;

        }
        public string ProductItemId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int? DiscountAmount { get; set; }
    }
}