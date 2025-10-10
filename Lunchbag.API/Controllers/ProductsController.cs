using AutoMapper;
using Lunchbag.API.Entities;
using Lunchbag.API.Models;
using Lunchbag.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lunchbag.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if(product == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct(ProductForCreationDto product)
        {
            var productToAdd = _mapper.Map<Product>(product);
            var category = await _productRepository.GetProductCategoryAsync(productToAdd);

            if (category == null)
            {
                return BadRequest("Product has no valid category");
            }

            productToAdd.Category = category;

            await _productRepository.AddAsync(productToAdd);
            await _productRepository.SaveChangesAsync();

            var productToReturn = _mapper.Map<ProductDto>(productToAdd);

            return CreatedAtRoute("GetProduct", new
            {
               id = productToReturn.Id,
            }, productToReturn);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, ProductForCreationDto product)
        {
            if (!await _productRepository.ExistsAsync(id))
            {
                return NotFound();
            }
            var productToUpdate = _mapper.Map<Product>(product);
            productToUpdate.Id = id;
            var category = await _productRepository.GetProductCategoryAsync(productToUpdate);

            if(category == null)
            {
                return BadRequest("Product has no valid category");
            }

            productToUpdate.Category = category;
            _productRepository.Update(productToUpdate);
            await _productRepository.SaveChangesAsync();

            var productToReturn = _mapper.Map<ProductDto>(productToUpdate);
            return CreatedAtRoute("GetProduct", new
            {
                id = productToReturn.Id,
            }, productToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null) {
                return NotFound(); 
            }

            _productRepository.Delete(product);
            await _productRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
