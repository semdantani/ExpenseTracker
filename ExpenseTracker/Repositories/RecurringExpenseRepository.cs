using ExpenseTracker.Interfaces;
using ExpenseTracker.Model;
using ExpenseTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repositories
{
    public class RecurringExpenseRepository : IRecurringExpenseRepository
    {
        private readonly AppDbContext _context;

        public RecurringExpenseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RecurringExpense Ex)
        {
            _context.RecurringExpenses.Add(Ex);
            await _context.SaveChangesAsync();
        }

        // Sirf active recurring expenses
        public async Task<List<RecurringExpense>> GetByUserIdAsync(int userId)
        {
            return await _context.RecurringExpenses
                .Where(x => x.UserId == userId && x.IsActive)
                .ToListAsync();
        }

        
        public async Task<List<RecurringExpense>> GetHistoryByUserIdAsync(int userId)
        {
            return await _context.RecurringExpenses
                .Where(x => x.UserId == userId && !x.IsActive)
                .ToListAsync();
        }

        public async Task<RecurringExpense?> GetByUserIdAndIdAsync(int ExID, int userId)
        {
            return await _context.RecurringExpenses
                .FirstOrDefaultAsync(e => e.Id == ExID && e.UserId == userId);
        }

        public async Task UpdateAsync(RecurringExpense Ex)
        {
            _context.RecurringExpenses.Update(Ex);
            await _context.SaveChangesAsync();
        }

        // Permanent delete
        // Purane expenses preserve honge
        public async Task DeletePermanentlyAsync(RecurringExpense Ex)
        {
            var expenses = await _context.Expenses
                .Where(e => e.RecurringExpenseId == Ex.Id)
                .ToListAsync();

            foreach (var expense in expenses)
            {
                expense.RecurringExpenseId = null;
            }

            _context.RecurringExpenses.Remove(Ex);

            await _context.SaveChangesAsync();
        }

       
        public async Task DeleteAsync(RecurringExpense Ex)
        {
            _context.RecurringExpenses.Remove(Ex);
            await _context.SaveChangesAsync();
        }
    }
}
