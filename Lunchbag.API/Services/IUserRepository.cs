using Lunchbag.API.Entities;

namespace Lunchbag.API.Services
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<bool> UserEmailExistsAsync(string email);
        public Task<User?> GetUserByEmail(string email);
    }
}
