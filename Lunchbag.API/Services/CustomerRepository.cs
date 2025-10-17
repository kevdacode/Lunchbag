using Lunchbag.API.DbContexts;
using Lunchbag.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lunchbag.API.Services
{
    public class CustomerRepository : UserRepository, ICustomerRepository
    {

        private readonly LunchbagContext _context;

        public CustomerRepository(LunchbagContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _context.Users
                             .OfType<Customer>()
                             .Where(c => c.Id == id)
                             .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Order>> GetCustomerOrdersAsync(int customerId)
        {
            var customer = await _context.Users
                .OfType<Customer>()
                .Where(u => u.Id == customerId)
                .Include(c => c.Orders)
                    .ThenInclude(os => os.OrderItems)
                        .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync();

            return customer!.Orders;
        }

        public async Task<ShoppingCart> GetCustomerShoppingCartAsync(int customerId)
        {
            var customer = await _context.Users
                .OfType<Customer>()
                .Where(u => u.Id == customerId)
                .Include(c => c.ShoppingCart)
                    .ThenInclude(sc => sc.CartItems)
                        .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync();

            return customer!.ShoppingCart;
        }
    }
}
