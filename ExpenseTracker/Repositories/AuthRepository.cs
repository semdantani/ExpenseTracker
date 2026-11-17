using ExpenseTracker.Data;
using ExpenseTracker.Interfaces;
using ExpenseTracker.Model;

namespace ExpenseTracker.Repositories
{
    public class AuthRepository : IAuthRepository
    {

        private readonly AppDbContext _context;
        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return _context.Users.FirstOrDefault(x => x.Email == email);
        }
    }
}
