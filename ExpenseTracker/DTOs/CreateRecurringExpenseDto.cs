using ExpenseTracker.Model;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.DTOs
{
    public class CreateRecurringExpenseDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Range(0.01,double.MaxValue)]
        public decimal Amount {  get; set; }

        public string? Description {  get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public RecurrenceFrequency Frequency { get; set; }

        [Required]
        public DateTime StartDate {  get; set; }

        public DateTime? EndDate { get; set; }   

    }
}
