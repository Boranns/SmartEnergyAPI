using SmartEnergyAPI.Models;

namespace SmartEnergyAPI.Repositories
{
    public interface IPortfolioRepository
    {
        Task<List<Portfolio>> GetByUserIdAsync(int userId);
        Task<Portfolio?> GetByUserAndProductAsync(int userId, int energyProductId);
        Task AddAsync(Portfolio portfolio);
        Task UpdateAsync(Portfolio portfolio);
        Task RemoveAsync(Portfolio portfolio);
        Task SaveChangesAsync();
    }
}
