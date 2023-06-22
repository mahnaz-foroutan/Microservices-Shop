using System.Collections.Generic;

namespace Shopping.Aggregator.Models
{
    public class BasketModel
    {
        public string Id { get; set; }

        public BasketModel(string id)
        {
            Id = id;
        }

        public string? BuyerEmail { get; set; }

        public List<BasketItemExtendedModel> Items { get; set; } = new List<BasketItemExtendedModel>();
        public int? DeliveryMethodId { get; set; }
        //public string? ClientSecret { get; set; }
        //public string? PaymentIntentId { get; set; }
        public decimal ShippingPrice { get; set; }
        public int? CouponTotal
        {
            get
            {
                int totalCoupon = 0;

                if (Items != null && Items.Any())
                {
                    foreach (BasketItemExtendedModel item in Items)
                    {
                        totalCoupon += item.DiscountAmount ?? 0;
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

                if (Items != null && Items.Any())
                {
                    foreach (BasketItemExtendedModel item in Items)
                    {
                        totalPrice += item.Price * item.Quantity;
                    }
                }

                return totalPrice;
            }
        }
    }
}
