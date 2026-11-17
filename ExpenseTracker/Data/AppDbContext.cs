using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Model;

namespace ExpenseTracker.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Expense>Expenses { get; set; }

        public DbSet<Budget> Budgets { get; set; }

        public DbSet<RecurringExpense>RecurringExpenses { get; set; }   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Expense>().HasIndex(e => new { e.RecurringExpenseId, e.Date })
                .IsUnique().HasFilter("[RecurringExpenseId] IS NOT NULL");
        }

    }
}
