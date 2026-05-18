using Moq;
using SmartEnergyAPI.DTOs;
using SmartEnergyAPI.Models;
using SmartEnergyAPI.Repositories;
using SmartEnergyAPI.Services;

namespace SmartEnergyAPI.Tests
{
    public class TradeServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IEnergyProductRepository> _energyProductRepositoryMock;
        private readonly Mock<ITradeRepository> _tradeRepositoryMock;
        private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock;
        private readonly TradeService _tradeService;

        public TradeServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _energyProductRepositoryMock = new Mock<IEnergyProductRepository>();
            _tradeRepositoryMock = new Mock<ITradeRepository>();
            _portfolioRepositoryMock = new Mock<IPortfolioRepository>();

            _tradeService = new TradeService(
                _userRepositoryMock.Object,
                _energyProductRepositoryMock.Object,
                _tradeRepositoryMock.Object,
                _portfolioRepositoryMock.Object
            );
        }

        [Fact]
        public async Task BuyEnergy_WithSufficientBalance_UpdatesBalance()
        {
            var user = new User { Id = 1, Balance = 10000, Username = "boran" };
            var product = new EnergyProduct { Id = 1, Name = "Solar", CurrentPrice = 45.50m };

            _userRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);
            _energyProductRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(product);
            _portfolioRepositoryMock.Setup(x => x.GetByUserAndProductAsync(1, 1)).ReturnsAsync((Portfolio?)null);

            var tradeDto = new TradeDto
            {
                EnergyProductId = 1,
                Type = "Buy",
                Quantity = 2
            };

            var result = await _tradeService.ExecuteTradeAsync(1, tradeDto);

            Assert.NotNull(result);
            Assert.Equal("Buy", result.Type);
            Assert.Equal(91m, result.Total);
            Assert.Equal(9909m, user.Balance);
        }

        [Fact]
        public async Task BuyEnergy_WithInsufficientBalance_ThrowsException()
        {
            var user = new User { Id = 1, Balance = 10, Username = "boran" };
            var product = new EnergyProduct { Id = 1, Name = "Solar", CurrentPrice = 45.50m };

            _userRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);
            _energyProductRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(product);

            var tradeDto = new TradeDto
            {
                EnergyProductId = 1,
                Type = "Buy",
                Quantity = 10
            };

            await Assert.ThrowsAsync<Exception>(() =>
                _tradeService.ExecuteTradeAsync(1, tradeDto));
        }

        [Fact]
        public async Task SellEnergy_WithoutPortfolio_ThrowsException()
        {
            var user = new User { Id = 1, Balance = 10000 };
            var product = new EnergyProduct { Id = 1, CurrentPrice = 45.50m };

            _userRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);
            _energyProductRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(product);
            _portfolioRepositoryMock.Setup(x => x.GetByUserAndProductAsync(1, 1)).ReturnsAsync((Portfolio?)null);

            var tradeDto = new TradeDto
            {
                EnergyProductId = 1,
                Type = "Sell",
                Quantity = 1
            };

            await Assert.ThrowsAsync<Exception>(() =>
                _tradeService.ExecuteTradeAsync(1, tradeDto));
        }

        [Fact]
        public async Task SellEnergy_WithSufficientQuantity_UpdatesBalance()
        {
            var user = new User { Id = 1, Balance = 5000 };
            var product = new EnergyProduct { Id = 1, CurrentPrice = 45.50m };
            var portfolio = new Portfolio { Id = 1, UserId = 1, EnergyProductId = 1, Quantity = 5 };

            _userRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);
            _energyProductRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(product);
            _portfolioRepositoryMock.Setup(x => x.GetByUserAndProductAsync(1, 1)).ReturnsAsync(portfolio);

            var tradeDto = new TradeDto
            {
                EnergyProductId = 1,
                Type = "Sell",
                Quantity = 2
            };

            var result = await _tradeService.ExecuteTradeAsync(1, tradeDto);

            Assert.NotNull(result);
            Assert.Equal("Sell", result.Type);
            Assert.Equal(91m, result.Total);
            Assert.Equal(5091m, user.Balance);
        }

        [Fact]
        public async Task InvalidTradeType_ThrowsException()
        {
            var user = new User { Id = 1, Balance = 10000 };
            var product = new EnergyProduct { Id = 1, CurrentPrice = 45.50m };

            _userRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);
            _energyProductRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(product);

            var tradeDto = new TradeDto
            {
                EnergyProductId = 1,
                Type = "Ugyldig",
                Quantity = 1
            };

            await Assert.ThrowsAsync<Exception>(() =>
                _tradeService.ExecuteTradeAsync(1, tradeDto));
        }
    }
}
