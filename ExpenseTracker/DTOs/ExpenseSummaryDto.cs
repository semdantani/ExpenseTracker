namespace ExpenseTracker.DTOs
{
    public class ExpenseSummaryDto
    {
        public decimal TotalExpense {  get; set; }
        public decimal MonthlyExpense {  get; set; }
        public int TotalTransactions {  get; set; }
        public string TopCategory { get; set; } = string.Empty;
    }
}
