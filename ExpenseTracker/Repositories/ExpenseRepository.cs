using ExpenseTracker.Data;
using ExpenseTracker.Interfaces;
using ExpenseTracker.Model;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly AppDbContext _context;
        public ExpenseRepository(AppDbContext context) { 
            _context = context;
        }
        public async Task AddAsync(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
        }

        public async Task<Expense?> GetByUserIdAndIdAsync(int expenseId, int userId)
        {
            return await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == expenseId && e.UserId == userId);
        }
        public async Task UpdateAsync(Expense expense)
        {
            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Expense expense)
        {
           _context.Expenses.Remove(expense);
           await _context.SaveChangesAsync();
        }

        public async Task<List<Expense>> GetByUserIdAsync(
            int userId,
            string?category,
            DateTime?startDate,
            DateTime?endDate,
            int pageNumber, 
            int pageSize,
            string sortBy,
            string sortOrder
            )
        {
            var query = _context.Expenses.Where(e => e.UserId == userId);
            if(!string.IsNullOrEmpty(category))
            {
                query=query.Where(e => e.Category == category);
            }
            if(startDate != null)
            {
                query=query.Where(e=>e.Date>=startDate);
            }
            if(endDate != null)
            {
                query=query.Where(e=>e.Date<=endDate);
            }
            if (sortBy == "amount")
            {
                if (sortOrder == "desc")
                {
                    query = query.OrderByDescending(e => e.Amount);
                }
                else
                {
                    query=query.OrderBy(e => e.Amount);
                }  
            }else if (sortBy == "date")
            {
                if (sortOrder == "desc") {
                    query = query.OrderByDescending(e => e.Date);
                    
                }
                else
                {
                    query = query.OrderBy(e => e.Date);
                }

            }
            else
            {
                query = query.OrderByDescending(e => e.Date);
            }

            query =query.Skip((pageNumber-1)*pageSize).Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<int> GetTotalCountAsync(int userId, string? category,DateTime? startDate,DateTime? endDate)
        {
            var query = _context.Expenses.Where(e => e.UserId == userId);
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(e => e.Category == category);
            }
            if (startDate != null)
            {
                query = query.Where(e => e.Date >= startDate);
            }
            if (endDate != null)
            {
                query = query.Where(e => e.Date <= endDate);
            }
           return await query.CountAsync();
        }

        public async Task AddRangeAsync(List<Expense> expenses)
        {
            _context.Expenses.AddRange(expenses);
            await _context.SaveChangesAsync();
        }
    }
}
