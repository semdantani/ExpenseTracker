using ExpenseTracker.DTOs;
using ExpenseTracker.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ExpenseTracker.Model;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseController(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateExpenseDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var expense = new Expense
            {
                Title = dto.Title,
                Amount = dto.Amount,
                Description = dto.Description,
                Date = dto.Date,
                Category = dto.Category,
                UserId = int.Parse(userId!),
            };

            await _expenseRepository.AddAsync(expense);
            return Ok(expense);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(string?category,DateTime?startDate,DateTime?endDate, int pageNumber = 1, int pageSize = 10,string sortBy="date",string sortOrder="desc")
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest("Invalid pagination parameters");
            }
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
                );

            var expenses=await _expenseRepository.GetByUserIdAsync(
                userId,
                category,
                startDate,
                endDate,
                pageNumber,
                pageSize,
                sortBy,
                sortOrder);
            var totalRecords = await _expenseRepository.GetTotalCountAsync(userId, category, startDate, endDate);
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            var currentPage = pageNumber;
            return Ok(new { expenses, totalRecords, totalPages, currentPage });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,[FromBody]UpdateExpenseDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var expense=await _expenseRepository.GetByUserIdAndIdAsync(id,userId);
            if(expense== null)
            {
                return NotFound("Empty");
            }
            expense.Title = dto.Title;
            expense.Amount = dto.Amount;
            expense.Description = dto.Description!;
            expense.Date = dto.Date;
            expense.Category = dto.Category;

           
            await _expenseRepository.UpdateAsync(expense);
            return Ok("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var expense = await _expenseRepository.GetByUserIdAndIdAsync(id, userId);
            if (expense == null) {
                return NotFound("Expense Not Exist");
            }
            await _expenseRepository.DeleteAsync(expense);

            return NoContent();

        }
    }
}
