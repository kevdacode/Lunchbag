using Lunchbag.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lunchbag.API.Services
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public Task<IEnumerable<Product>> GetCategoryProductsAsync(int id);
    }
}
