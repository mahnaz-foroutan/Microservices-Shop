using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Ordering.Api.EventBusConsumer;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Domain.Entities.Identity;
using Xunit;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Ordering.Api.Extensions;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.User.Dtos;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Test
{
    public class BasketCheckoutConsumerTests
    {
        private readonly BasketCheckoutConsumer _consumer;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<ILogger<BasketCheckoutConsumer>> _mockLogger;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<UserManager<AppUser>> _mockUserManager;

        public BasketCheckoutConsumerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockMediator = new Mock<IMediator>();
            _mockLogger = new Mock<ILogger<BasketCheckoutConsumer>>();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockUserManager = new Mock<UserManager<AppUser>>();

            _consumer = new BasketCheckoutConsumer(
                _mockMapper.Object,
                _mockMediator.Object,
                _mockLogger.Object,
                _mockOrderRepository.Object,
                _mockUserManager.Object
            );
        }

        //[Fact]
        //public async Task Consume_ValidBasketCheckoutEvent_CreatesOrderAndLogsSuccess()
        //{
        //    // Arrange
        //    var eventMessage = new BasketCheckoutEvent
        //    {
        //        // Set necessary properties of the event message
        //    };

        //    var command = new CheckoutOrderCommand
        //    {
        //        // Set necessary properties of the command
        //    };

        //    var user = new AppUser
        //    {
        //        Email = "test@example.com",
        //        Address = new Domain.Entities.Identity.Address
        //        {
        //            // Set necessary properties of the address
        //        }
        //    };

        //    var expectedOrderResult = 1;

        //    _mockMapper.Setup(mapper => mapper.Map<CheckoutOrderCommand>(eventMessage)).Returns(command);
        //    _mockUserManager.Setup(manager => manager.Users.Include(It.IsAny<Func<IQueryable<AppUser>, IQueryable<object>>>()))
        //        .Returns(MockUsersDbSet(new List<AppUser> { user }));

        //    _mockMediator.Setup(mediator => mediator.Send(command)).ReturnsAsync(expectedOrderResult);

        //    // Act
        //    await _consumer.Consume(new ConsumeContext<BasketCheckoutEvent>(eventMessage));

        //    // Assert
        //    _mockMapper.Verify(mapper => mapper.Map<CheckoutOrderCommand>(eventMessage), Times.Once);
        //    _mockMediator.Verify(mediator => mediator.Send(command), Times.Once);
        //    _mockLogger.Verify(logger => logger.LogInformation(
        //        $"order consumed successfully and order id is : {expectedOrderResult}"),
        //        Times.Once
        //    );
        //}

        //[Fact]
        //public async Task Consume_InvalidBasketCheckoutEvent_LogsError()
        //{
        //    // Arrange
        //    var eventMessage = new BasketCheckoutEvent
        //    {
        //        // Set necessary properties of the event message
        //    };

        //    var command = new CheckoutOrderCommand
        //    {
        //        // Set necessary properties of the command
        //    };

        //    var user = new AppUser
        //    {
        //        Email = "test@example.com",
        //        Address = new Address
        //        {
        //            // Set necessary properties of the address
        //        }
        //    };

        //    _mockMapper.Setup(mapper => mapper.Map<CheckoutOrderCommand>(eventMessage)).Returns(command);
        //    _mockUserManager.Setup(manager => manager.Users.Include(It.IsAny<Func<IQueryable<AppUser>, IQueryable<object>>>()))
        //        .Returns(MockUsersDbSet(new List<AppUser> { user }));

        //    _mockMediator.Setup(mediator => mediator.Send(command)).ThrowsAsync(new Exception("Order creation failed"));

        //    // Act
        //    await _consumer.Consume(new ConsumeContext<BasketCheckoutEvent>(eventMessage));

        //    // Assert
        //    _mockMapper.Verify(mapper => mapper.Map<CheckoutOrderCommand>(eventMessage), Times.Once);
        //    _mockMediator.Verify(mediator => mediator.Send(command), Times.Once);
        //    _mockLogger.Verify(logger => logger.LogInformation(
        //        $"order consumed error and order id is : {It.IsAny<Exception>()}"),
        //        Times.Once
        //    );
        //}

        private static DbSet<T> MockUsersDbSet<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            return dbSetMock.Object;
        }
    }
}
