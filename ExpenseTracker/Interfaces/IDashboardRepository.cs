using ExpenseTracker.DTOs;

namespace ExpenseTracker.Interfaces
{
    public interface IDashboardRepository
    {
        Task <ExpenseSummaryDto> GetSummaryAsync(int userId);
        Task <List<MonthlyExpenseDto>>GetMonthlyBreakdownAsync(int userId);
        Task <List<CategoryExpenseDto>> GetCategoryBreakdownAsync(int userId);
    }
}
