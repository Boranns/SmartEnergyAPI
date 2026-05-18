using SmartEnergyAPI.DTOs;
using SmartEnergyAPI.Models;

namespace SmartEnergyAPI.Services
{
    public interface ITradeService
    {
        Task<Trade> ExecuteTradeAsync(int userId, TradeDto dto);
        Task<List<Trade>> GetUserTradesAsync(int userId);
        Task<List<Trade>> GetAllTradesAsync();
    }
}
