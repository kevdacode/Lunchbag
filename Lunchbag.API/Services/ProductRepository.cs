using Lunchbag.API.DbContexts;
using Lunchbag.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lunchbag.API.Services
{
    public class ProductRepository : IProductRepository
    {

        private readonly LunchbagContext _context;

        public ProductRepository(LunchbagContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(Product entity)
        {
            await _context.Products.AddAsync(entity);
        }

        public void Delete(Product entity)
        {
            _context.Products.Remove(entity);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.Where(p => p.Id == id).Include(p => p.Category).FirstOrDefaultAsync();
        }

        public async Task<Category?> GetProductCategoryAsync(Product product)
        {
            return await _context.Categories.Where(c => c.Id == product.CategoryId).FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public void Update(Product entity)
        {
            _context.Products.Update(entity);
        }
    }
}
