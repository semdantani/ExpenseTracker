using ExpenseTracker.Data;
using ExpenseTracker.DTOs;
using ExpenseTracker.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _Context;

        public DashboardRepository(AppDbContext context)
        {
            _Context = context;
        }

        public async Task<List<MonthlyExpenseDto>> GetMonthlyBreakdownAsync(int userId)
        {
            var monthlyExpense =await _Context.Expenses
                .Where(e => e.UserId == userId)
                .GroupBy(e => new { e.Date.Year, e.Date.Month })
                .Select(g =>new MonthlyExpenseDto
                {
                   Month=$"{g.Key.Year}-{g.Key.Month}", 
                   TotalExpense=g.Sum(e=>e.Amount)
                })
                .ToListAsync();
            return monthlyExpense;
        }
        public async Task<List<CategoryExpenseDto>>GetCategoryBreakdownAsync(int userId)
        {
            var categoryExpense=await _Context.Expenses
                .Where (e => e.UserId == userId)
                .GroupBy(e=>e.Category)
                .Select(g=>new CategoryExpenseDto
                {
                    Category=g.Key,
                    TotalExpense=g.Sum(e=>e.Amount)
                })
                .ToListAsync() ;
            return categoryExpense;
        }

        public async Task<ExpenseSummaryDto>GetSummaryAsync(int userId)
        {
            var totalExpense =await _Context.Expenses.
                Where(e => e.UserId == userId)
                .SumAsync(e => e.Amount);

            var today= DateTime.Today;
            var startDate = new DateTime(today.Year, today.Month, 1);
            var endDate = startDate.AddMonths(1);

            var monthlyExpense =await _Context.Expenses
                .Where(e => e.UserId == userId && e.Date >= startDate && e.Date < endDate)
                .SumAsync(e => e.Amount);

            var totalTransactions=await _Context.Expenses
                .CountAsync(e=>e.UserId == userId);
            var topCategory = await _Context.Expenses
                .Where(e => e.UserId == userId)
                .GroupBy(e => e.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalAmount = g.Sum(e => e.Amount)
                })
                .OrderByDescending(x => x.TotalAmount)
                .FirstOrDefaultAsync();

            var summary = new ExpenseSummaryDto
            {
                TotalExpense = totalExpense,
                MonthlyExpense=monthlyExpense,
                TotalTransactions=totalTransactions,
                TopCategory=topCategory?.Category??"No Expense"
            };

            return summary;
        }
    }
}
