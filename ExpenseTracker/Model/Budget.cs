using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Model
{
    public class Budget
    {
        public int Id { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        [Range(typeof(decimal),"0.01", "79228162514264337593543950335")]
        public Decimal Amount { get; set; }

        [Required]
        [Range(1,12)]
        public int Month { get; set; }

        [Required]
        [Range(2000,2100)]
        public int Year {  get; set; }

        [Required]
        public int UserId {  get; set; }
        public User? User { get; set; }

    }
}
