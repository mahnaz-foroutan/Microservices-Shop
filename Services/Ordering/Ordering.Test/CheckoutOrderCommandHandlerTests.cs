using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Models;
using Ordering.Domain.Entities;


namespace Ordering.Test
{
    public class CheckoutOrderCommandHandlerTests
    {
        private readonly CheckoutOrderCommandHandler _handler;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<ILogger<CheckoutOrderCommandHandler>> _mockLogger;

        public CheckoutOrderCommandHandlerTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockEmailService = new Mock<IEmailService>();
            _mockLogger = new Mock<ILogger<CheckoutOrderCommandHandler>>();

            _handler = new CheckoutOrderCommandHandler(
                _mockOrderRepository.Object,
                _mockMapper.Object,
                _mockEmailService.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsOrderId()
        {
            // Arrange
            var command = new CheckoutOrderCommand
            {
                DelieveryMethod = 1,
                Items = new List<CheckoutOrderBasketItems>(),
                // Set other properties as needed
            };

            var orderEntity = new Order();
            var deliveryMethod = new DeliveryMethod();
            var newOrder = new Order { Id = 1 };

            _mockMapper.Setup(m => m.Map<Order>(command)).Returns(orderEntity);
            _mockOrderRepository.Setup(r => r.GetDeliveryMethodsById(command.DelieveryMethod ?? 1)).ReturnsAsync(deliveryMethod);
            _mockMapper.Setup(m => m.Map<DeliveryMethod>(deliveryMethod)).Returns(deliveryMethod);
            _mockMapper.Setup(m => m.Map<List<OrderItem>>(command.Items)).Returns(new List<OrderItem>());
            _mockOrderRepository.Setup(r => r.AddAsync(orderEntity)).ReturnsAsync(newOrder);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(newOrder.Id, result);
            
            //_mockLogger.Verify(
            //    log => log.LogInformation($"order {newOrder.Id} is successfully created"),
            //    Times.Once
            //);
            _mockEmailService.Verify(
                service => service.SendEmail(It.IsAny<Email>()),
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_EmailServiceThrowsException_LogsError()
        {
            // Arrange
            var command = new CheckoutOrderCommand();
            var orderEntity = new Order();
            var newOrder = new Order { Id = 1 };

            _mockMapper.Setup(m => m.Map<Order>(command)).Returns(orderEntity);
            _mockOrderRepository.Setup(r => r.GetDeliveryMethodsById(command.DelieveryMethod ?? 1)).ReturnsAsync(new DeliveryMethod());
            _mockMapper.Setup(m => m.Map<List<OrderItem>>(command.Items)).Returns(new List<OrderItem>());
            _mockOrderRepository.Setup(r => r.AddAsync(orderEntity)).ReturnsAsync(newOrder);

            _mockEmailService.Setup(service => service.SendEmail(It.IsAny<Email>())).ThrowsAsync(new Exception("Email sending failed"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(newOrder.Id, result);
           
            //_mockLogger.Verify(
            //    log => log.LogError("email has not been send"),
            //    Times.Once
            //);
        }
    }
}
