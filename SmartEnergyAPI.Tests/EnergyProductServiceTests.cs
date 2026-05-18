using Moq;
using SmartEnergyAPI.Models;
using SmartEnergyAPI.Repositories;
using SmartEnergyAPI.Services;

namespace SmartEnergyAPI.Tests
{
    public class EnergyProductServiceTests
    {
        private readonly Mock<IEnergyProductRepository> _energyProductRepositoryMock;
        private readonly Mock<HttpClient> _httpClientMock;
        private readonly EnergyProductService _energyProductService;

        public EnergyProductServiceTests()
        {
            _energyProductRepositoryMock = new Mock<IEnergyProductRepository>();
            _httpClientMock = new Mock<HttpClient>();

            _energyProductService = new EnergyProductService(
                _energyProductRepositoryMock.Object,
                _httpClientMock.Object
            );
        }

        [Fact]
        public async Task GetAll_ReturnsAllProducts()
        {
            var products = new List<EnergyProduct>
            {
                new EnergyProduct { Id = 1, Name = "Solar", Symbol = "SOL", CurrentPrice = 45.50m },
                new EnergyProduct { Id = 2, Name = "Wind", Symbol = "WIN", CurrentPrice = 38.20m },
                new EnergyProduct { Id = 3, Name = "Gas", Symbol = "GAS", CurrentPrice = 82.10m },
                new EnergyProduct { Id = 4, Name = "Nuclear", Symbol = "NUC", CurrentPrice = 55.00m }
            };

            _energyProductRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(products);

            var result = await _energyProductService.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public async Task GetById_WithValidId_ReturnsProduct()
        {
            var product = new EnergyProduct { Id = 1, Name = "Solar", Symbol = "SOL", CurrentPrice = 45.50m };

            _energyProductRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(product);

            var result = await _energyProductService.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Solar", result.Name);
            Assert.Equal(45.50m, result.CurrentPrice);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ReturnsNull()
        {
            _energyProductRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((EnergyProduct?)null);

            var result = await _energyProductService.GetByIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task SeedEnergyProducts_WhenProductsExist_DoesNotAddProducts()
        {
            _energyProductRepositoryMock
                .Setup(x => x.AnyAsync())
                .ReturnsAsync(true);

            await _energyProductService.SeedEnergyProductsAsync();

            _energyProductRepositoryMock.Verify(
                x => x.AddRangeAsync(It.IsAny<List<EnergyProduct>>()),
                Times.Never);
        }

        [Fact]
        public async Task SeedEnergyProducts_WhenNoProducts_AddsProducts()
        {
            _energyProductRepositoryMock
                .Setup(x => x.AnyAsync())
                .ReturnsAsync(false);

            await _energyProductService.SeedEnergyProductsAsync();

            _energyProductRepositoryMock.Verify(
                x => x.AddRangeAsync(It.IsAny<List<EnergyProduct>>()),
                Times.Once);
        }
    }
}
