using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Model
{
    public class RecurringExpense
    {

        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        public string? Description { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public RecurrenceFrequency Frequency { get; set; } = RecurrenceFrequency.Monthly;

        public DateTime StartDate {  get; set; }

        public DateTime? EndDate {  get; set; }

     
        public DateTime NextDueDate {  get; set; }

   
        public bool IsActive { get; set; } = true;

        [Required]
        public int UserId {  get; set; }

    }
}
