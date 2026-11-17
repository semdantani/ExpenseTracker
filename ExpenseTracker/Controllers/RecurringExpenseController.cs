using ExpenseTracker.DTOs;
using ExpenseTracker.Interfaces;
using ExpenseTracker.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RecurringExpenseController : ControllerBase
    {
        private readonly IRecurringExpenseRepository _recurringExReop;

        public RecurringExpenseController(IRecurringExpenseRepository recurringExReop)
        {
            _recurringExReop = recurringExReop;
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> CreateRecurringExpense(CreateRecurringExpenseDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (dto.EndDate.HasValue && dto.EndDate.Value < dto.StartDate)
            {
                return BadRequest("End Date cannot be earlier than Start Date");
            }

            var RecurringEx = new RecurringExpense()
            {
                Title = dto.Title,
                Amount = dto.Amount,
                Description = dto.Description,
                Category = dto.Category,
                Frequency = dto.Frequency,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                NextDueDate = dto.StartDate,
                UserId = userId,
                IsActive = true
            };

            await _recurringExReop.AddAsync(RecurringEx);

            return Ok(RecurringEx);
        }

        // GET ALL ACTIVE RECURRING EXPENSES
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _recurringExReop.GetByUserIdAsync(userId);

            return Ok(result);
        }

        // GET HISTORY (INACTIVE RECORDS)
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _recurringExReop.GetHistoryByUserIdAsync(userId);

            return Ok(result);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _recurringExReop.GetByUserIdAndIdAsync(id, userId);

            if (result == null)
            {
                return NotFound("Recurring Expense Not Found");
            }

            return Ok(result);
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecurringExpense(int id, UpdateRecurringExpenseDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _recurringExReop.GetByUserIdAndIdAsync(id, userId);

            if (result == null || !result.IsActive)
            {
                return NotFound("Active Recurring Expense Not Found");
            }

            if (dto.EndDate.HasValue && dto.EndDate.Value < dto.StartDate)
            {
                return BadRequest("End Date cannot be earlier than Start Date");
            }

            result.Title = dto.Title;
            result.Amount = dto.Amount;
            result.Description = dto.Description;
            result.Category = dto.Category;
            result.Frequency = dto.Frequency;
            result.StartDate = dto.StartDate;
            result.EndDate = dto.EndDate;
            result.NextDueDate = dto.StartDate;

            await _recurringExReop.UpdateAsync(result);

            return Ok(result);
        }

        // SOFT DELETE (DEACTIVATE)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecurringExpense(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _recurringExReop.GetByUserIdAndIdAsync(id, userId);

            if (result == null)
            {
                return NotFound("Recurring Expense Not Found");
            }

            result.IsActive = false;

            await _recurringExReop.UpdateAsync(result);

            return NoContent();
        }

        // RESTORE INACTIVE RECURRING EXPENSE
        [HttpPut("restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _recurringExReop.GetByUserIdAndIdAsync(id, userId);

            if (result == null || result.IsActive)
            {
                return NotFound("Inactive Recurring Expense Not Found");
            }

            result.IsActive = true;

            await _recurringExReop.UpdateAsync(result);

            return Ok(result);
        }
        // PERMANENT DELETE FROM HISTORY
        [HttpDelete("permanent/{id}")]
        public async Task<IActionResult> DeletePermanently(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _recurringExReop.GetByUserIdAndIdAsync(id, userId);

            if (result == null || result.IsActive)
            {
                return NotFound("Inactive Recurring Expense Not Found");
            }

            await _recurringExReop.DeletePermanentlyAsync(result);

            return NoContent();
        }
    }
}
