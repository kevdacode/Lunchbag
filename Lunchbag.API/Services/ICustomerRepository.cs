using Lunchbag.API.Entities;

namespace Lunchbag.API.Services
{
    public interface ICustomerRepository : IUserRepository
    {
        public Task<Customer?> GetCustomerByIdAsync(int id);
        public Task<ShoppingCart> GetCustomerShoppingCartAsync(int customerId);
        public Task<IEnumerable<Order>> GetCustomerOrdersAsync(int customerId);
    }
}
