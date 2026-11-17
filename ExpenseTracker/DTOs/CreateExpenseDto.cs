using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.DTOs
{
    public class CreateExpenseDto
    {
        [Required]
        public string Title{ get; set; } = string.Empty;

        [Required]
        [Range(0.01,double.MaxValue)]
        public Decimal Amount {  get; set; }

        public string? Description { get; set;}
        public DateTime Date {  get; set;}
        [Required]
        public string Category {  get; set;}=string.Empty;
    }
}
