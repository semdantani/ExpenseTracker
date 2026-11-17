using ExpenseTracker.DTOs;
using ExpenseTracker.Model;
namespace ExpenseTracker.Interfaces
{
    public interface IBudgetRepository
    {
        Task AddAsync(Budget budget);
        Task<List<Budget>> GetByUserIdAsync(int userId);
        Task <Budget?> GetByUserIdAndIdAsync(int budgetId, int userId);
        Task UpdateAsync(Budget budget);
        Task DeleteAsync(Budget budget);

        Task <List<BudgetStatusDto>> GetBudgetStatusAsync(int userId, int month, int year);
        Task <bool> ExistsAsync(int userId,string category,int month,int year);
        Task<bool> ExistsForUpdateAsync(int Id,int userId,string category,int month,int year);
    }
}
