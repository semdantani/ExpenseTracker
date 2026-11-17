
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Model
{
    public class Expense
    {
        public int Id { get; set; }

        public int? RecurringExpenseId { get; set; }

        public RecurringExpense? RecurringExpense { get; set; }

        [Required]
        public string Title { get; set; }=string.Empty;

        [Required]
        [Range(0.01,double.MaxValue)]
        public decimal Amount { get; set; }

        public string? Description {  get; set; }=string.Empty;

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Category {  get; set; }=string.Empty;

        public int UserId {  get; set; }
        public User User { get; set; } = null!;
        
    }
}
