using Lunchbag.API.DbContexts;
using Lunchbag.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Lunchbag.API.Services
{
    public class CustomerRepository : ICustomerRepository
    {

        private readonly LunchbagContext _context;

        public CustomerRepository(LunchbagContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(Customer entity)
        {
            await _context.Customers.AddAsync(entity);
        }

        public async Task<bool> CustomerMailExistsAsync(string email)
        {
            return await _context.Customers.Where(c => c.Email == email).AnyAsync();
        }

        public void Delete(Customer entity)
        {
            _context.Customers.Remove(entity);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Customers.AnyAsync(c  => c.Id == id);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Customer?> GetCustomerByEmail(string email)
        {
            return await _context.Customers.Where(c =>c.Email == email).FirstOrDefaultAsync();  
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public void Update(Customer entity)
        {
            _context.Update(entity);
        }
    }
}
