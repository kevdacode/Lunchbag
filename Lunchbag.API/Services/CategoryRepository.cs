using Lunchbag.API.DbContexts;
using Lunchbag.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lunchbag.API.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LunchbagContext _context;

        public CategoryRepository(LunchbagContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(Category entity)
        {
            await _context.Categories.AddAsync(entity);
        }

        public void Delete(Category entity)
        {
            _context.Categories.Remove(entity);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetCategoryProductsAsync(int id)
        {
            return await _context.Products
                .Where(p => p.CategoryId == id)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public void Update(Category entity)
        {
            _context.Categories.Update(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
