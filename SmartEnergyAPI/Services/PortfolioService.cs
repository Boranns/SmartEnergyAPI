using SmartEnergyAPI.Models;
using SmartEnergyAPI.Repositories;

namespace SmartEnergyAPI.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IEnergyProductRepository _energyProductRepository;

        public PortfolioService(IPortfolioRepository portfolioRepository, IEnergyProductRepository energyProductRepository)
        {
            _portfolioRepository = portfolioRepository;
            _energyProductRepository = energyProductRepository;
        }

        public async Task<List<Portfolio>> GetUserPortfolioAsync(int userId)
        {
            return await _portfolioRepository.GetByUserIdAsync(userId);
        }

        public async Task<decimal> GetTotalPortfolioValueAsync(int userId)
        {
            var portfolios = await _portfolioRepository.GetByUserIdAsync(userId);
            return portfolios.Sum(p => p.Quantity * p.EnergyProduct.CurrentPrice);
        }
    }
}