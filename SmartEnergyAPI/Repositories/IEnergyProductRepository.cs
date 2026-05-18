using SmartEnergyAPI.Models;

namespace SmartEnergyAPI.Repositories
{
    public interface IEnergyProductRepository
    {
        Task<List<EnergyProduct>> GetAllAsync();
        Task<EnergyProduct?> GetByIdAsync(int id);
        Task<EnergyProduct?> GetBySymbolAsync(string symbol);
        Task<bool> AnyAsync();
        Task AddRangeAsync(List<EnergyProduct> products);
        Task UpdateAsync(EnergyProduct product);
        Task SaveChangesAsync();
    }
}
