using AutoMapper;
using Catalog.Api.Controllers;
using Catalog.Api.Dtos;
using Catalog.Api.Errors;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Test
{
    public class CatalogControllerTests
    {
        private readonly CatalogController _controller;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<ILogger<CatalogController>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;

        public CatalogControllerTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<CatalogController>>();
            _mockMapper = new Mock<IMapper>();
            
            _controller = new CatalogController(
                _mockProductRepository.Object,
                _mockLogger.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task GetProducts_ReturnsListOfProducts()
        {
            // Arrange
            var products = new List<Product> { new Product { Id = "1" }, new Product { Id = "2" } };

            _mockProductRepository.Setup(repo => repo.GetProductsAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _controller.GetProductsAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Equal(2, returnedProducts.Count());
        }

        [Fact]
        public async Task GetProduct_ReturnsProduct_WhenFound()
        {
            // Arrange
            var product = new Product { Id = "1" };
            var productDto = new ProductToReturnDto { Id = "1" };

            _mockProductRepository.Setup(repo => repo.GetProductByIdAsync("1"))
                .ReturnsAsync(product);
            _mockMapper.Setup(mapper => mapper.Map<ProductToReturnDto>(product))
                .Returns(productDto);

            // Act
            var result = await _controller.GetProduct("1");

            // Assert
           var returnedProductDto = Assert.IsAssignableFrom<ProductToReturnDto>(result.Value);
            Assert.Equal("1", returnedProductDto.Id);
        }

        [Fact]
        public async Task GetProduct_ReturnsNotFound_WhenProductNotFound()
        {
            // Arrange
            _mockProductRepository.Setup(repo => repo.GetProductByIdAsync("1"))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _controller.GetProduct("1");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse>(notFoundResult.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
            Assert.Equal(StatusCodes.Status404NotFound, apiResponse.StatusCode);
        }

        [Fact]
        public async Task GetBrands_ReturnsListOfBrands()
        {
            // Arrange
            var brands = new List<ProductBrand> { new ProductBrand { Id ="1", Name = "Brand 1" }, new ProductBrand { Id = "2", Name = "Brand 2" } };

            _mockProductRepository.Setup(repo => repo.GetProductBrandsAsync())
                .ReturnsAsync(brands);

            // Act
            var result = await _controller.GetBrands();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBrands = Assert.IsAssignableFrom<IEnumerable<ProductBrand>>(okResult.Value);
            Assert.Equal(2, returnedBrands.Count());
        }

        [Fact]
        public async Task GetTypes_ReturnsListOfTypes()
        {
            // Arrange
            var types = new List<ProductType> { new ProductType { Id ="1", Name = "Type 1" }, new ProductType { Id = "2", Name = "Type 2" } };

            _mockProductRepository.Setup(repo => repo.GetProductTypesAsync())
                .ReturnsAsync(types);

            // Act
            var result = await _controller.GetTypes();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTypes = Assert.IsAssignableFrom<IEnumerable<ProductType>>(okResult.Value);
            Assert.Equal(2, returnedTypes.Count());
        }

        [Fact]
        public async Task CreateProduct_ReturnsCreatedResponse_WithProduct()
        {
            // Arrange
            var product = new Product { Id = "1", Name = "Product 1" };

            _mockProductRepository.Setup(repo => repo.CreateProduct(product))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateProduct(product);

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
            Assert.Equal("GetProduct", createdAtRouteResult.RouteName);
            Assert.Equal("1", createdAtRouteResult.RouteValues["id"]);
            var returnedProduct = Assert.IsAssignableFrom<Product>(createdAtRouteResult.Value);
            Assert.Equal("Product 1", returnedProduct.Name);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsOkResult_WhenProductUpdatedSuccessfully()
        {
            // Arrange
            var product = new Product { Id = "1", Name = "Updated Product" };

            _mockProductRepository.Setup(repo => repo.UpdateProduct(product))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateProduct(product);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsOkResult_WhenProductDeletedSuccessfully()
        {
            // Arrange
            string productId = "1";

            _mockProductRepository.Setup(repo => repo.DeleteProduct(productId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteProduct(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteProductAll_ReturnsOkResult_WhenAllProductsDeletedSuccessfully()
        {
            // Arrange
            _mockProductRepository.Setup(repo => repo.DeleteAll())
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteProductAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

    }
}
