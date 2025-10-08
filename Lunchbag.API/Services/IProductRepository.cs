using Lunchbag.API.Entities;

namespace Lunchbag.API.Services
{
    public interface IProductRepository : IRepository<Product>
    {
        public Task<Category?> GetProductCategoryAsync(Product product);
    }
}
