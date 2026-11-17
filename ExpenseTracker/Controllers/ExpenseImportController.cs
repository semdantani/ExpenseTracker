using ExpenseTracker.Interfaces;
using ExpenseTracker.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Globalization;
using CsvHelper;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseImportController : ControllerBase
    {
        private readonly IExpenseRepository _expenseRepo;

        public ExpenseImportController(IExpenseRepository expenseRepo)
        {
            _expenseRepo = expenseRepo;
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            // 1. File validation
            if (file == null || file.Length == 0)
                return BadRequest("Please upload a CSV file.");

            if (!string.Equals(
                Path.GetExtension(file.FileName),
                ".csv",
                StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Only CSV files are allowed.");
            }

            if (file.Length > 5 * 1024 * 1024)
                return BadRequest("File size cannot exceed 5 MB.");

            // 2. Create CSV reader (CsvHelper 34.0.0)
            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = CsvReader.Create(reader);

            // 3. Read headers
            var headers = csv.Headers
                .Select(h => h.Trim().TrimStart('\uFEFF'))
                .ToArray();

            var requiredHeaders = new[]
            {
                "Title",
                "Amount",
                "Description",
                "Date",
                "Category"
            };

            if (!requiredHeaders.All(required =>
                headers.Any(header =>
                    string.Equals(
                        header,
                        required,
                        StringComparison.OrdinalIgnoreCase))))
            {
                return BadRequest(new
                {
                    Message = "Invalid CSV headers.",
                    FoundHeaders = headers,
                    RequiredHeaders = requiredHeaders
                });
            }

            // 4. Get logged-in user ID
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var validExpenses = new List<Expense>();
            var rowErrors = new List<string>();

            int rowNumber = 1;

            // 5. Read each data row
            try
            {
                while (csv.Read())
                {
                    rowNumber++;

                    string title = csv.GetString("Title").Trim();
                    string amountText = csv.GetString("Amount").Trim();
                    string description = csv.GetString("Description").Trim();
                    string dateText = csv.GetString("Date").Trim();
                    string category = csv.GetString("Category").Trim();

                    // Validate title
                    if (string.IsNullOrWhiteSpace(title))
                    {
                        rowErrors.Add($"Row {rowNumber}: Title is required.");
                        continue;
                    }

                    // Validate amount
                    if (!decimal.TryParse(
                        amountText,
                        NumberStyles.Number,
                        CultureInfo.InvariantCulture,
                        out decimal amount) || amount <= 0)
                    {
                        rowErrors.Add($"Row {rowNumber}: Invalid amount.");
                        continue;
                    }

                    // Validate date
                    if (!DateTime.TryParse(
                        dateText,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime date))
                    {
                        rowErrors.Add($"Row {rowNumber}: Invalid date.");
                        continue;
                    }

                    // Validate category
                    if (string.IsNullOrWhiteSpace(category))
                    {
                        rowErrors.Add($"Row {rowNumber}: Category is required.");
                        continue;
                    }

                    // 6. Create Expense
                    var expense = new Expense
                    {
                        Title = title,
                        Amount = amount,
                        Description = description,
                        Date = date,
                        Category = category,
                        UserId = userId
                    };

                    validExpenses.Add(expense);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Error while reading CSV data.",
                    Error = ex.Message
                });
            }

            // 7. Save valid expenses
            if (validExpenses.Count > 0)
            {
                await _expenseRepo.AddRangeAsync(validExpenses);
            }

            // 8. Return import result
            return Ok(new
            {
                ImportedCount = validExpenses.Count,
                FailedCount = rowErrors.Count,
                Errors = rowErrors
            });
        }
    }
}
