using ExpenseTracker.Data;
using ExpenseTracker.DTOs;
using ExpenseTracker.Interfaces;
using ExpenseTracker.Model;
using Microsoft.EntityFrameworkCore;


namespace ExpenseTracker.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly AppDbContext _context;
        public BudgetRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Budget budget)
        {
            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Budget budget)
        {
            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Budget>> GetByUserIdAsync(int userId)
        {
            var result =await _context.Budgets
                .Where(e => e.UserId == userId)
                .ToListAsync();
            return result;
        }

        public async Task <Budget?> GetByUserIdAndIdAsync(int budgetId, int userId)
        {
            return await _context.Budgets
                .FirstOrDefaultAsync(e => e.Id == budgetId && e.UserId == userId);
           
        }

        public async Task UpdateAsync(Budget budget)
        {
            _context.Budgets.Update(budget);
            await _context.SaveChangesAsync();
        }

        public async Task<List<BudgetStatusDto>> GetBudgetStatusAsync(int userId, int month, int year)
        {
            var budgets =await _context.Budgets
                .Where(b => b.UserId == userId && b.Month == month && b.Year == year)
                .ToListAsync();
            var result=new List<BudgetStatusDto>();
            
            foreach(var budget in budgets)
            {
                var actualExpense =await _context.Expenses
                    .Where(e => e.UserId == userId &&
                              e.Category == budget.Category &&
                              e.Date.Month == month &&
                              e.Date.Year == year
                    ).SumAsync(e => e.Amount);
                result.Add(new BudgetStatusDto
                {
                    Category = budget.Category,
                    BudgetAmount = budget.Amount,
                    ActualExpense = actualExpense,
                    RemainingAmount = budget.Amount - actualExpense,
                    IsExceeded = actualExpense > budget.Amount
                });
            }
            return result;
        }

        public async Task<bool> ExistsAsync(int userId, string category, int month, int year)
        {
            return await _context.Budgets.AnyAsync(b=>
            b.UserId==userId &&
            b.Category==category&&
            b.Month==month &&
            b.Year==year);
        }
        public async Task<bool> ExistsForUpdateAsync(int Id,int userId,string category, int month, int year)
        {
            return await _context.Budgets.AnyAsync(b =>
              b.Id != Id &&
              b.UserId == userId &&
              b.Category == category &&
              b.Month == month &&
              b.Year == year
            );
        }
    }
}
