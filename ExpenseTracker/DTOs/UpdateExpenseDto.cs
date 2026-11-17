using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.DTOs
{
    public class UpdateExpenseDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Range(0.01,double.MaxValue)]
        public decimal Amount {  get; set; }

        public string? Description {  get; set; }

        public DateTime Date {  get; set; }
        [Required]
        public string Category {  get; set; }= string.Empty;

    }
}
