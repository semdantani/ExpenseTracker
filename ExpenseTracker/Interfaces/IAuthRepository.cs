using ExpenseTracker.Model;

namespace ExpenseTracker.Interfaces
{
    public interface IAuthRepository
    {
        Task <User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
    }
}
