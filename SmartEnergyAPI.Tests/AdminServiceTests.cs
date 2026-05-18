using Moq;
using SmartEnergyAPI.Models;
using SmartEnergyAPI.Repositories;
using SmartEnergyAPI.Services;

namespace SmartEnergyAPI.Tests
{
    public class AdminServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ITradeRepository> _tradeRepositoryMock;
        private readonly AdminService _adminService;

        public AdminServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _tradeRepositoryMock = new Mock<ITradeRepository>();

            _adminService = new AdminService(
                _userRepositoryMock.Object,
                _tradeRepositoryMock.Object
            );
        }

        [Fact]
        public async Task GetAllUsers_ReturnsAllUsers()
        {
            var users = new List<User>
            {
                new User { Id = 1, Username = "boran", Email = "boran@gmail.com", Role = "Admin", Balance = 10000, IsActive = true },
                new User { Id = 2, Username = "trader1", Email = "trader1@gmail.com", Role = "Trader", Balance = 5000, IsActive = true }
            };

            _userRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(users);

            var result = await _adminService.GetAllUsersAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetUserById_WithValidId_ReturnsUser()
        {
            var user = new User { Id = 1, Username = "boran", Email = "boran@gmail.com", Role = "Admin", Balance = 10000 };

            _userRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

            var result = await _adminService.GetUserByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("boran", result.Username);
            Assert.Equal("Admin", result.Role);
        }

        [Fact]
        public async Task GetUserById_WithInvalidId_ThrowsException()
        {
            _userRepositoryMock.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((User?)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _adminService.GetUserByIdAsync(99));
        }

        [Fact]
        public async Task ToggleUserStatus_ActiveUser_DeactivatesUser()
        {
            var user = new User { Id = 1, Username = "boran", IsActive = true };

            _userRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

            var result = await _adminService.ToggleUserStatusAsync(1);

            Assert.False(result);
            Assert.False(user.IsActive);
        }

        [Fact]
        public async Task ToggleUserStatus_InactiveUser_ActivatesUser()
        {
            var user = new User { Id = 1, Username = "boran", IsActive = false };

            _userRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

            var result = await _adminService.ToggleUserStatusAsync(1);

            Assert.True(result);
            Assert.True(user.IsActive);
        }

        [Fact]
        public async Task GetDashboardStats_ReturnsCorrectStats()
        {
            var users = new List<User>
            {
                new User { Id = 1, IsActive = true },
                new User { Id = 2, IsActive = true },
                new User { Id = 3, IsActive = false }
            };

            var trades = new List<Trade>
            {
                new Trade { Id = 1, Total = 100m },
                new Trade { Id = 2, Total = 200m },
                new Trade { Id = 3, Total = 300m }
            };

            _userRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(users);
            _tradeRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(trades);

            var result = await _adminService.GetDashboardStatsAsync();

            Assert.NotNull(result);

            var stats = result.GetType().GetProperties();
            var totalUsers = (int)result.GetType().GetProperty("totalUsers")!.GetValue(result)!;
            var activeUsers = (int)result.GetType().GetProperty("activeUsers")!.GetValue(result)!;
            var totalTrades = (int)result.GetType().GetProperty("totalTrades")!.GetValue(result)!;
            var totalVolume = (decimal)result.GetType().GetProperty("totalVolume")!.GetValue(result)!;

            Assert.Equal(3, totalUsers);
            Assert.Equal(2, activeUsers);
            Assert.Equal(3, totalTrades);
            Assert.Equal(600m, totalVolume);
        }
    }
}
