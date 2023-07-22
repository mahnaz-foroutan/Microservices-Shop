using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Api.Dtos;
using Catalog.Core.Entities;
using Catalog.Api.Helpers;
using Catalog.Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using Catalog.Api.Errors;
using Catalog.Core.Interfaces;

namespace Catalog.Api.Controllers
{
    public class CatalogController : BaseApiController
    {
        #region constructor
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;
        private readonly IMapper _mapper;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger, IMapper mapper)
        {
            _productRepository = productRepository;
            _logger = logger;
            _mapper = mapper;
        }

        #endregion

        #region get products
        [HttpGet("getAll")]
       // [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsAll()
        {
            var products  = await _productRepository.GetProductsAsync();
               
            return Ok(products);
        }

        #endregion



       
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery] ProductSpecParams productParams)
        {
           
                var totalItems = _productRepository.Count(productParams);

            var products = _productRepository.ListAsync(productParams);

            var data = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex,
                productParams.PageSize, totalItems, data));
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(string id)
        {
            
                var product = await _productRepository.GetProductByIdAsync(id);

                if (product == null) return NotFound(new ApiResponse(404));
           
            return _mapper.Map<ProductToReturnDto>(product);
        }

      
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            return Ok(await _productRepository.GetProductBrandsAsync());
        }

      
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            return Ok(await _productRepository.GetProductTypesAsync());
        }
        /// <summary>
        /// //
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        #region create product

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _productRepository.CreateProduct(product);
         
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        #endregion

        #region update product

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _productRepository.UpdateProduct(product));
        }

        #endregion

        #region delete product

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
           
            return Ok(await _productRepository.DeleteProduct(id));
        }

        [HttpDelete( Name = "DeleteProductAll")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductAll()
        {
            return Ok(await _productRepository.DeleteAll());
        }

        #endregion
    }
}
