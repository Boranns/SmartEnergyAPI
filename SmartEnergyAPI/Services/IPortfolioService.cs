using SmartEnergyAPI.Models;

namespace SmartEnergyAPI.Services
{
    public interface IPortfolioService
    {
        Task<List<Portfolio>> GetUserPortfolioAsync(int userId);
        Task<decimal> GetTotalPortfolioValueAsync(int userId);
    }
}