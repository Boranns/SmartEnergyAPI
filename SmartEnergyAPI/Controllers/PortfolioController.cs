using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEnergyAPI.Services;
using System.Security.Claims;

namespace SmartEnergyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;

        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyPortfolio()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var portfolio = await _portfolioService.GetUserPortfolioAsync(userId);
            return Ok(portfolio);
        }

        [HttpGet("total-value")]
        public async Task<IActionResult> GetTotalValue()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var totalValue = await _portfolioService.GetTotalPortfolioValueAsync(userId);
            return Ok(new { totalValue });
        }
    }
}
