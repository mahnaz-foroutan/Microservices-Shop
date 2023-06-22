using AutoMapper;
using Basket.Api.Entities;
using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Basket.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        #region constructor

        private readonly IBasketRepository _basketRepository;
       private readonly DiscountGrpcService _discountService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;


        public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountService, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _basketRepository = basketRepository;
            _discountService = discountService;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        #endregion

        #region get basket

        [HttpGet("{id}", Name = "basket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasketById(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new ShoppingCart(id));
        }

        #endregion

        #region update basket
        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            // todo: get data from discount.grpc and calculate final price of product

            foreach (var item in basket.Items)
            {
                var copun = await _discountService.GetDiscount(item.ProductId);
                item.Price -= copun.Amount;
                item.DiscountAmount = copun.Amount;
            }

            return Ok(await _basketRepository.UpdateBasketAsync(basket));
        }

        #endregion

        #region remove basket

        [HttpDelete("{id}", Name = "basket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
            return Ok();
        }

        #endregion

        #region checkout
        [HttpPost("[action]")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<bool>> Checkout([FromBody] ShoppingCart basketc)
        {
            try
            {
                // get existing basket with total price
                // var basket = await _basketRepository.(basketCheckout.UserName);
                var basket = await _basketRepository.GetBasketAsync(basketc.Id);
                if (basket == null)
                {
                    return BadRequest();
                }


                // create BasketCheckoutEvent -- set total price on basketCheckout event message
                var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketc);
                eventMessage.Subtotal = basket.Subtotal;

                // send checkout event to rabbitmq
                await _publishEndpoint.Publish(eventMessage);

                // remove basket
                await _basketRepository.DeleteBasketAsync(basketc.Id);
                return true;
            }
           catch
            {
                return false;
            }
        }

        #endregion
    }
}
