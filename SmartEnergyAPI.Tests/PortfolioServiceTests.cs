using Moq;
using SmartEnergyAPI.Models;
using SmartEnergyAPI.Repositories;
using SmartEnergyAPI.Services;

namespace SmartEnergyAPI.Tests
{
    public class PortfolioServiceTests
    {
        private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock;
        private readonly Mock<IEnergyProductRepository> _energyProductRepositoryMock;
        private readonly PortfolioService _portfolioService;

        public PortfolioServiceTests()
        {
            _portfolioRepositoryMock = new Mock<IPortfolioRepository>();
            _energyProductRepositoryMock = new Mock<IEnergyProductRepository>();

            _portfolioService = new PortfolioService(
                _portfolioRepositoryMock.Object,
                _energyProductRepositoryMock.Object
            );
        }

        [Fact]
        public async Task GetUserPortfolio_ReturnsUserPortfolio()
        {
            var portfolio = new List<Portfolio>
            {
                new Portfolio { Id = 1, UserId = 1, EnergyProductId = 1, Quantity = 2 },
                new Portfolio { Id = 2, UserId = 1, EnergyProductId = 2, Quantity = 5 }
            };

            _portfolioRepositoryMock
                .Setup(x => x.GetByUserIdAsync(1))
                .ReturnsAsync(portfolio);

            var result = await _portfolioService.GetUserPortfolioAsync(1);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetUserPortfolio_WithNoPortfolio_ReturnsEmptyList()
        {
            _portfolioRepositoryMock
                .Setup(x => x.GetByUserIdAsync(1))
                .ReturnsAsync(new List<Portfolio>());

            var result = await _portfolioService.GetUserPortfolioAsync(1);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetTotalPortfolioValue_CalculatesCorrectly()
        {
            var energyProduct1 = new EnergyProduct { Id = 1, CurrentPrice = 45.50m };
            var energyProduct2 = new EnergyProduct { Id = 2, CurrentPrice = 38.20m };

            var portfolio = new List<Portfolio>
            {
                new Portfolio { Id = 1, UserId = 1, EnergyProductId = 1, Quantity = 2, EnergyProduct = energyProduct1 },
                new Portfolio { Id = 2, UserId = 1, EnergyProductId = 2, Quantity = 3, EnergyProduct = energyProduct2 }
            };

            _portfolioRepositoryMock
                .Setup(x => x.GetByUserIdAsync(1))
                .ReturnsAsync(portfolio);

            var result = await _portfolioService.GetTotalPortfolioValueAsync(1);

            Assert.Equal(205.60m, result);
        }

        [Fact]
        public async Task GetTotalPortfolioValue_WithEmptyPortfolio_ReturnsZero()
        {
            _portfolioRepositoryMock
                .Setup(x => x.GetByUserIdAsync(1))
                .ReturnsAsync(new List<Portfolio>());

            var result = await _portfolioService.GetTotalPortfolioValueAsync(1);

            Assert.Equal(0m, result);
        }
    }
}
