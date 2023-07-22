using MassTransit.Testing;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Test
{
    public class RabbitMQMessagingTests : IDisposable
    {
        private readonly IBusControl _bus;
        private readonly InMemoryTestHarness _harness;

        public RabbitMQMessagingTests()
        {
            _harness = new InMemoryTestHarness();
            _bus = Bus.Factory.CreateUsingInMemory(cfg =>
            {
                cfg.ReceiveEndpoint(_harness.InputQueueName, ep =>
                {
                    ep.Handler<YourMessage>(context => Task.CompletedTask);
                });
            });

            _bus.Start();
        }

        [Fact]
        public async Task PublishAndConsumeMessage_Success()
        {
            // Arrange
            var message = new YourMessage { Id = 1, Text = "Hello, World!" };
            await _harness.Start();
            // Act
            await _bus.Publish(message);

            // Assert
            var f = await _harness.Published.Any<YourMessage>();
            Assert.False(f);
            var g = await _harness.Consumed.Any<YourMessage>();
            Assert.False(g);
        }

        public void Dispose()
        {
            _bus.Stop();
            _harness.Stop();
        }
    }

    public class YourMessage
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }

}
