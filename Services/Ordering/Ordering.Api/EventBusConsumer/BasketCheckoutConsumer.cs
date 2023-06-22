using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Api.Extensions;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.User.Dtos;
using Ordering.Domain.Entities;
using Ordering.Domain.Entities.Identity;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Api.EventBusConsumer
{
    public class BasketCheckoutConsumer : ControllerBase,IConsumer<BasketCheckoutEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<BasketCheckoutConsumer> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<AppUser> _userManager;

        public BasketCheckoutConsumer(IMapper mapper,IMediator mediator, ILogger<BasketCheckoutConsumer> logger, 
            IOrderRepository orderRepository, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
            _orderRepository = orderRepository;
            _userManager = userManager;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            try
            {
                // get basket from repo
                var command = _mapper.Map<CheckoutOrderCommand>(context.Message);
                var address = await _userManager.Users.Include(x => x.Address)
                    .SingleOrDefaultAsync(x => x.Email == command.BuyerEmail);
                command.ShipToAddress = _mapper.Map<AddressDto>(address.Address);

                // calc subtotal
                command.Subtotal = command.Items.Sum(item => item.Price * item.Quantity);
                command.PaymentIntentId = "Payment Received";
                // check to see if order exists
                // var spec = new OrderByPaymentIntentWithItemsSpecification(command.PaymentIntentId);
                int result = 0;

                result = await _mediator.Send(command);

                _logger.LogInformation($"order consumed successfully and order id is : {result}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"order consumed error and order id is :{ex} ");
               
            }
        }

    }
}
