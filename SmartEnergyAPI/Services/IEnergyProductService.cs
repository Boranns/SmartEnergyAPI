using SmartEnergyAPI.Models;

namespace SmartEnergyAPI.Services
{
    public interface IEnergyProductService
    {
        Task<List<EnergyProduct>> GetAllAsync();
        Task<EnergyProduct?> GetByIdAsync(int id);
        Task SeedEnergyProductsAsync();
        Task UpdatePricesFromEnergidataAsync();
    }
}
