using ExpenseTracker.DTOs;
using ExpenseTracker.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Model;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetRepository _budgetRepo;
        public BudgetController(IBudgetRepository budgetRepo)
        {
            _budgetRepo = budgetRepo;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBudgetDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            bool Exists = await _budgetRepo.ExistsAsync(
                userId,
                dto.Category,
                dto.Month,
                dto.Year
                );
            if (Exists)
            {
                return BadRequest("Budget already exists for this category and month.");
            }
            var budget = new Budget()
            {
                Category = dto.Category,
                Amount = dto.Amount,
                Month = dto.Month,
                Year = dto.Year,
                UserId = userId,
            };
            await _budgetRepo.AddAsync(budget);
            return Ok(budget);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result=await _budgetRepo.GetByUserIdAsync(userId);
            return Ok(result);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result=await _budgetRepo.GetByUserIdAndIdAsync(id, userId);
            if(result== null)
            {
                return NotFound("Budget NoT Found");
            }
            
            return Ok(result);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id,UpdateBudgetDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result=await _budgetRepo.GetByUserIdAndIdAsync(id,userId);

            if( result== null)
            {
                return NotFound("Budget Not Found");
            }
            bool exists =await _budgetRepo.ExistsForUpdateAsync(
                id,
                userId,
                dto.Category,
                dto.Month,
                dto.Year
                );

            if(exists)
            {
                return BadRequest("Budget already exists for this category and month.");
            }

            result.Category = dto.Category;
            result.Amount = dto.Amount;
            result.Month = dto.Month;
            result.Year = dto.Year;
           
            await _budgetRepo.UpdateAsync(result);
            return Ok(result);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result=await _budgetRepo.GetByUserIdAndIdAsync(id,userId);

            if (result == null)
            {
                return NotFound("Budget Not Found");
            }
            await _budgetRepo.DeleteAsync(result);
            return NoContent();
        }
        [HttpGet("status")]
        public async Task<IActionResult> GetBudgetStatus(int month,int year)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if(month<1||month>12 || year <2000 || year > 2100)
            {
                return BadRequest("Invalid month of year");
            }

            var result=await _budgetRepo.GetBudgetStatusAsync(userId,month,year);
            return Ok(result);
        }
    }
}
