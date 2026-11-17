using ExpenseTracker.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace ExpenseTracker.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepository _deshboardRepository;

        public DashboardController(IDashboardRepository repository)
        {
            _deshboardRepository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetSummary()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var summary=await _deshboardRepository.GetSummaryAsync(userId);
            return Ok(summary);
        }
        [HttpGet("monthly-breakdown")]
        public async Task<IActionResult> GetMonthlyBreakdown()
        {
            var userId=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result=await _deshboardRepository.GetMonthlyBreakdownAsync(userId);
            return Ok(result);
        }
        [HttpGet("category-breakdown")]
        public async Task<IActionResult> GetCategoryBreakdown()
        {
            var userId=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result=await _deshboardRepository.GetCategoryBreakdownAsync(userId);
            return Ok(result );
        }
    }
}
