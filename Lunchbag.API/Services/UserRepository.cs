using Lunchbag.API.DbContexts;
using Lunchbag.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Lunchbag.API.Services
{
    public class UserRepository : IUserRepository
    {

        private readonly LunchbagContext _context;

        public UserRepository(LunchbagContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
        }

        public async Task<bool> UserEmailExistsAsync(string email)
        {
            return await _context.Users.Where(u => u.Email == email).AnyAsync();
        }

        public void Delete(User entity)
        {
            _context.Users.Remove(entity);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(c  => c.Id == id);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.Where(u =>u.Email == email).FirstOrDefaultAsync();  
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public void Update(User entity)
        {
            _context.Users.Update(entity);
        }
    }
}
