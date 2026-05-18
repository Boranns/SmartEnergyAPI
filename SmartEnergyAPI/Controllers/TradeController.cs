using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEnergyAPI.DTOs;
using SmartEnergyAPI.Services;
using System.Security.Claims;

namespace SmartEnergyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TradeController : ControllerBase
    {
        private readonly ITradeService _tradeService;

        public TradeController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteTrade([FromBody] TradeDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var trade = await _tradeService.ExecuteTradeAsync(userId, dto);
                return Ok(trade);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("my-trades")]
        public async Task<IActionResult> GetMyTrades()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var trades = await _tradeService.GetUserTradesAsync(userId);
            return Ok(trades);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllTrades()
        {
            var trades = await _tradeService.GetAllTradesAsync();
            return Ok(trades);
        }
    }
}
