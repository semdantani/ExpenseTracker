using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.DTOs
{
    public class UpdateBudgetDto
    {
        [Required]
        public string Category {  get; set; }=string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount {  get; set; }

        [Range(1,12)]
        public int Month {  get; set; }
        [Range(2000,2100)]
        public int Year {  get; set; }
    }
}
