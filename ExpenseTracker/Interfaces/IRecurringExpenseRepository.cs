
using ExpenseTracker.Model;

namespace ExpenseTracker.Interfaces
{
    public interface IRecurringExpenseRepository
    {
        Task AddAsync(RecurringExpense Ex);

        Task<List<RecurringExpense>> GetByUserIdAsync(int userId);

        Task<RecurringExpense?> GetByUserIdAndIdAsync(int ExID, int userId);

        Task<List<RecurringExpense>> GetHistoryByUserIdAsync(int userId);

        Task UpdateAsync(RecurringExpense Ex);

        Task DeleteAsync(RecurringExpense Ex);

        Task DeletePermanentlyAsync(RecurringExpense Ex);
    }
}

