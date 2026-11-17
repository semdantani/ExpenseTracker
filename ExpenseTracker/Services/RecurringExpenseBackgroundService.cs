using ExpenseTracker.Data;
using ExpenseTracker.Model;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
    public class RecurringExpenseBackgroundService:BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public RecurringExpenseBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using(var scope = _scopeFactory.CreateAsyncScope())
                {
                    var context=scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var dueExpenses=await context.RecurringExpenses
                        .Where(e=>e.IsActive && e.NextDueDate <= DateTime.UtcNow)
                        .ToListAsync(stoppingToken);
                    foreach (var item in dueExpenses)
                    {
                        while (item.IsActive && item.NextDueDate <= DateTime.UtcNow)
                        {
                            if (item.EndDate.HasValue && item.NextDueDate > item.EndDate.Value)
                            {
                                item.IsActive = false;
                                break;
                            }
                            bool alreadyExists =await context.Expenses.AnyAsync(e =>
                            e.RecurringExpenseId == item.Id &&
                            e.Date == item.NextDueDate);

                            if (!alreadyExists)
                            {
                                var expense = new Expense
                                {
                                    Title = item.Title,
                                    Amount = item.Amount,
                                    Description = item.Description,
                                    Category = item.Category,
                                    Date = item.NextDueDate,
                                    UserId = item.UserId,
                                    RecurringExpenseId = item.Id,
                                };

                                context.Expenses.Add(expense);

                            }

                            switch (item.Frequency)
                            {
                                case RecurrenceFrequency.Daily:
                                    item.NextDueDate = item.NextDueDate.AddDays(1);
                                    break;

                                case RecurrenceFrequency.Weekly:
                                    item.NextDueDate = item.NextDueDate.AddDays(7);
                                    break;

                                case RecurrenceFrequency.Monthly:
                                    item.NextDueDate = item.NextDueDate.AddMonths(1);
                                    break;

                                case RecurrenceFrequency.Yearly:
                                    item.NextDueDate = item.NextDueDate.AddYears(1);
                                    break;
                                default:
                                    item.IsActive = false;
                                    break;

                            }

                            if (item.EndDate.HasValue && item.NextDueDate > item.EndDate.Value)
                            {
                                item.IsActive = false;
                            }
                        }
                    }
                    await context.SaveChangesAsync(stoppingToken);
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

        }
    }
}
