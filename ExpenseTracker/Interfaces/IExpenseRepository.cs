using ExpenseTracker.Model;

namespace ExpenseTracker.Interfaces
{
    public interface IExpenseRepository
    {
        Task AddAsync(Expense expense);
        Task<List<Expense>> GetByUserIdAsync(
            int userId,
            string? category,
            DateTime?startDate,
            DateTime?endDate,
            int pageNumber,
            int pageSize,
            string sortBy,
            string sortOrder
            );

        Task<Expense?> GetByUserIdAndIdAsync(int expenseId, int userId);


        Task UpdateAsync(Expense expense);
        Task DeleteAsync(Expense expense);
        Task<int> GetTotalCountAsync(int userId, string? category, DateTime? startDate, DateTime? endDate);

        Task AddRangeAsync(List<Expense> expenses);

    }
}
