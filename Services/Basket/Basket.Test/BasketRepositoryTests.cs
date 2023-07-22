using Basket.Api.Entities;
using Basket.Api.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Basket.Test
{
  
    public class BasketRepositoryTests
    {
        private readonly BasketRepository _basketRepository;
        private readonly Mock<IDistributedCacheWrapper> _mockRedisCache;

        public BasketRepositoryTests()
        {
            // Create a mock instance of IDistributedCache
            _mockRedisCache = new Mock<IDistributedCacheWrapper>();

            // Initialize the BasketRepository with the mock cache
            _basketRepository = new BasketRepository(_mockRedisCache.Object);
        }

        [Fact]
        public async Task GetBasketAsync_ExistingBasketId_ReturnsBasket()
        {
            // Arrange
            string basketId = "123";
            var basket = new ShoppingCart ( basketId );

            // Serialize the basket object
            var serializedBasket = JsonConvert.SerializeObject(basket);

            // Mock the GetStringAsync method of the cache to return the serialized basket
            _mockRedisCache.Setup(c => c.GetStringAsync(basketId))
                .ReturnsAsync(serializedBasket);

            // Act
            var result = await _basketRepository.GetBasketAsync(basketId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(basketId, result.Id);
        }

        [Fact]
        public async Task GetBasketAsync_NonExistingBasketId_ReturnsNull()
        {
            // Arrange
            string basketId = "123";

         
            // Mock the GetStringAsync method of the cache to return null
            _mockRedisCache.Setup(c => c.GetStringAsync(basketId))
                .ReturnsAsync((string)null);

            // Act
            var result = await _basketRepository.GetBasketAsync(basketId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateBasketAsync_ValidBasket_UpdatesBasketAndReturnsUpdatedBasket()
        {
            // Arrange
            var basketId = "123";
            var basket = new ShoppingCart(basketId);

            // Serialize the basket object
            var serializedBasket = JsonConvert.SerializeObject(basket);
            string storedBasket = null;
            _mockRedisCache.Setup(c => c.SetStringAsync(basket.Id, serializedBasket))
                .Returns(Task.CompletedTask);

            // Mock the GetStringAsync method to return the captured serialized basket
            _mockRedisCache.Setup(c => c.GetStringAsync(basketId))
                .ReturnsAsync(() => serializedBasket);

            // Act
            var result = await _basketRepository.UpdateBasketAsync(basket);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(basket.Id, result.Id);
        }

        [Fact]
        public async Task DeleteBasketAsync_ValidBasketId_DeletesBasket()
        {
            // Arrange
            string basketId = "123";

            // Mock the RemoveAsync method of the cache
            _mockRedisCache.Setup(c => c.RemoveAsync(basketId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _basketRepository.DeleteBasketAsync(basketId);

            // Assert
            Assert.True(result);
        }
    }
}
