namespace ExpenseTracker.DTOs
{
    public class BudgetStatusDto
    {
        public string Category {  get; set; }
        public decimal BudgetAmount {  get; set; }
        public decimal ActualExpense {  get; set; }

        public decimal RemainingAmount {  get; set; }
        public bool IsExceeded { get; set; }
    }
}
