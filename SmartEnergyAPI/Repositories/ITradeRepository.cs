using SmartEnergyAPI.Models;

namespace SmartEnergyAPI.Repositories
{
    public interface ITradeRepository
    {
        Task<List<Trade>> GetByUserIdAsync(int userId);
        Task<List<Trade>> GetAllAsync();
        Task AddAsync(Trade trade);
        Task SaveChangesAsync();
    }
}
