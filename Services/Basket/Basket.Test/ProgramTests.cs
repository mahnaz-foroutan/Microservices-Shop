using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Builder;
using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Basket.Test
{
    public class ProgramTests : IClassFixture<WebApplicationFactory<Program>>
    {
       // private readonly TestServer _server;
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;
        public ProgramTests(WebApplicationFactory<Program> factory)
        {
            // Arrange
            _factory = factory;
            _client = _factory.CreateClient();
            
           
        }

        [Fact]
        public async Task GetSwaggerEndpoint_ReturnsSwaggerUIPage()
        {
            // Act
            var response = await _client.GetAsync("/swagger/index.html");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void ConfigureServices_RegistersBasketRepository()
        {
            // Arrange
            var services = new ServiceCollection();
            var builder = new ConfigurationBuilder();
            var configuration = builder.Build();
            // Act
            var mockBasketRepository = new Mock<IBasketRepository>();
            var application = new WebApplicationFactory<Program>()
         .WithWebHostBuilder(builder =>
         {
             builder.ConfigureServices(services =>
             {
                 services.AddScoped<IBasketRepository>(provider => mockBasketRepository.Object);
                 var serviceProvider = services.BuildServiceProvider();
                 var resolvedBasketRepository = serviceProvider.GetService<IBasketRepository>();
                 // Assert
                 Assert.Same(mockBasketRepository.Object, resolvedBasketRepository);
             });
         });
            
        }

        [Fact]
        public void ConfigureServices_RegistersDiscountGrpcService()
        {
            // Arrange
            var services = new ServiceCollection();
            var builder = new ConfigurationBuilder();
            var configuration = builder.Build();

            // Act
            var application = new WebApplicationFactory<Program>()
          .WithWebHostBuilder(builder =>
          {
              builder.ConfigureServices(services =>
              {
                  var serviceProvider = services.BuildServiceProvider();
                  var resolvedDiscountGrpcService = serviceProvider.GetService<DiscountGrpcService>();
                  // Assert
                  Assert.NotNull(resolvedDiscountGrpcService);
              });
          });
            
        }
    }
}
