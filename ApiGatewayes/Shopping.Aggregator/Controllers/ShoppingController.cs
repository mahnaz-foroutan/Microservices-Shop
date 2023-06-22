using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System.Net;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;

        public ShoppingController(ICatalogService catalogService, IBasketService basketService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
        }

        [HttpGet( Name = "GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string id)
        {
            var basket = await _basketService.GetBasketAsync(id);

            if (basket != null)
            {
                //foreach (var item in basket.Items)
                //{
                //    var product = await _catalogService.GetCatalog(item.ProductId);

                //    item.ProductName = product.Name;
                //    item.Category = product.Category;
                //    item.Summary = product.Summary;
                //    item.Description = product.Description;
                //    item.ImageFile = product.ImageFile;
                //}
            }

           // var orders = await _orderService.GetOrderByUserName(userName);

            var shoppingModel = new ShoppingModel
            {
               // UserName = userName,
                BasketWithProduct = basket,
               // Orders = orders
            };

            return Ok(shoppingModel);
        }
    }
}
