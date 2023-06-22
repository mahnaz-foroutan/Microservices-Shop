using System.Collections.Generic;
using System.Linq;

namespace Basket.Api.Entities
{
    public class ShoppingCart
    {
        public string Id { get; set; }

        public ShoppingCart(string id)
        {
            Id = id;
        }

        public string? BuyerEmail { get; set; }

        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
        public int? DeliveryMethodId { get; set; }
        //public string? ClientSecret { get; set; }
        //public string? PaymentIntentId { get; set; }
        public decimal ShippingPrice { get; set; }
        public int? CouponTotal {
            get
            {
                int totalCoupon = 0;

                if (Items != null && Items.Any())
                {
                    foreach (ShoppingCartItem item in Items)
                    {
                        totalCoupon += item.DiscountAmount??0;
                    }
                }

                return totalCoupon;
            }

        }
        public decimal Subtotal
        {
            get
            {
                decimal totalPrice = 0;
                
                if(Items != null && Items.Any())
                {
                    foreach (ShoppingCartItem item in Items)
                    {
                        totalPrice += item.Price * item.Quantity;
                    }
                }

                return totalPrice;
            }
        }

    }
}
