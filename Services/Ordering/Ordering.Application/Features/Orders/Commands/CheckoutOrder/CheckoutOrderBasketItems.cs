using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderBasketItems
    {
        public CheckoutOrderBasketItems()
        {
        }

        public CheckoutOrderBasketItems(int quantity,decimal price, string productId, string productName, string? pictureUrl, string brand, string type, int? discountAmount)
        {
            Quantity = quantity;
            Price = price;
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
            Brand = brand;
            Type = type;
            DiscountAmount = discountAmount;
        }
        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string ProductId { get; set; }

        public string ProductName { get; set; }
        public string? PictureUrl { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public int? DiscountAmount { get; set; }
    }
}
