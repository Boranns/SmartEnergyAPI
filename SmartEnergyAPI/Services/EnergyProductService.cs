using SmartEnergyAPI.Models;
using SmartEnergyAPI.Repositories;
using System.Text.Json;

namespace SmartEnergyAPI.Services
{
    public class EnergyProductService : IEnergyProductService
    {
        private readonly IEnergyProductRepository _energyProductRepository;
        private readonly HttpClient _httpClient;

        public EnergyProductService(IEnergyProductRepository energyProductRepository, HttpClient httpClient)
        {
            _energyProductRepository = energyProductRepository;
            _httpClient = httpClient;
        }

        public async Task<List<EnergyProduct>> GetAllAsync()
        {
            return await _energyProductRepository.GetAllAsync();
        }

        public async Task<EnergyProduct?> GetByIdAsync(int id)
        {
            return await _energyProductRepository.GetByIdAsync(id);
        }

        public async Task SeedEnergyProductsAsync()
        {
            if (await _energyProductRepository.AnyAsync()) return;

            var products = new List<EnergyProduct>
            {
                new EnergyProduct { Name = "Solar", Symbol = "SOL", CurrentPrice = 45.50m, PreviousPrice = 44.00m, Unit = "MWh" },
                new EnergyProduct { Name = "Wind", Symbol = "WIN", CurrentPrice = 38.20m, PreviousPrice = 39.50m, Unit = "MWh" },
                new EnergyProduct { Name = "Gas", Symbol = "GAS", CurrentPrice = 82.10m, PreviousPrice = 80.00m, Unit = "MWh" },
                new EnergyProduct { Name = "Nuclear", Symbol = "NUC", CurrentPrice = 55.00m, PreviousPrice = 54.00m, Unit = "MWh" }
            };

            await _energyProductRepository.AddRangeAsync(products);
            await _energyProductRepository.SaveChangesAsync();
        }

        public async Task UpdatePricesFromEnergidataAsync()
        {
            try
            {
                var url = "https://api.energidataservice.dk/dataset/Elspotprices?limit=1&filter={\"PriceArea\":\"DK1\"}";
                var response = await _httpClient.GetStringAsync(url);

                var json = JsonDocument.Parse(response);
                var records = json.RootElement.GetProperty("records");

                if (records.GetArrayLength() > 0)
                {
                    var firstRecord = records[0];
                    var spotPrice = firstRecord.GetProperty("SpotPriceDKK").GetDecimal();

                    var windProduct = await _energyProductRepository.GetBySymbolAsync("WIN");

                    if (windProduct != null)
                    {
                        windProduct.PreviousPrice = windProduct.CurrentPrice;
                        windProduct.CurrentPrice = spotPrice / 1000;
                        windProduct.LastUpdated = DateTime.UtcNow;
                        await _energyProductRepository.UpdateAsync(windProduct);
                        await _energyProductRepository.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Priserne kunne ikke opdateres: {ex.Message}");
            }
        }
    }
}
