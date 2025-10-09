using Lunchbag.API.Entities;

namespace Lunchbag.API.Services
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        public Task<bool> CustomerMailExistsAsync(string email);
        public Task<Customer?> GetCustomerByEmail(string email);
    }
}
