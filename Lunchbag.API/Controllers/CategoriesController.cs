using AutoMapper;
using Lunchbag.API.Entities;
using Lunchbag.API.Models;
using Lunchbag.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lunchbag.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<CategoryDto>>(categories));
        }

        [HttpGet("{id}", Name = "GetCategory")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if(category == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CategoryDto>(category));
        }

        [HttpGet("{id}/products")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetCategoryProducts(int id)
        {
            if(!await _categoryRepository.ExistsAsync(id))
            {
                return NotFound();
            }

            var products = await _categoryRepository.GetCategoryProductsAsync(id);
            return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory(CategoryDto category)
        {
            if(await _categoryRepository.ExistsAsync(category.Id))
            {
                return Conflict("Category already exists");
            }

            var categoryToAdd = _mapper.Map<Category>(category);

            await _categoryRepository.AddAsync(categoryToAdd);
            await _categoryRepository.SaveChangesAsync();

            var categoryToReturn = _mapper.Map<CategoryDto>(categoryToAdd);

            return CreatedAtRoute("GetCategory", new
            {
                id = categoryToReturn.Id,
            }, categoryToReturn);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, CategoryDto category)
        {
            if(!await _categoryRepository.ExistsAsync(id))
            {
                return NotFound();
            }

            var categoryToUpdate = _mapper.Map<Category>(category);
            categoryToUpdate.Id = id;

            _categoryRepository.Update(categoryToUpdate);
            await _categoryRepository.SaveChangesAsync();

            var categoryToReturn = _mapper.Map<CategoryDto>(categoryToUpdate);

            return CreatedAtRoute("GetCategory", new
            {
                id = categoryToReturn.Id,
            }, categoryToReturn);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
      
            if(category == null)
            {
                return NotFound();
            }

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
