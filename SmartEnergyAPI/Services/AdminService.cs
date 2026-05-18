using SmartEnergyAPI.DTOs;
using SmartEnergyAPI.Repositories;

namespace SmartEnergyAPI.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITradeRepository _tradeRepository;

        public AdminService(IUserRepository userRepository, ITradeRepository tradeRepository)
        {
            _userRepository = userRepository;
            _tradeRepository = tradeRepository;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role,
                Balance = u.Balance
            }).ToList();
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id)
                ?? throw new Exception("Brugeren blev ikke fundet.");

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Balance = user.Balance
            };
        }

        public async Task<bool> ToggleUserStatusAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id)
                ?? throw new Exception("Brugeren blev ikke fundet.");

            user.IsActive = !user.IsActive;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return user.IsActive;
        }

        public async Task<object> GetDashboardStatsAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var trades = await _tradeRepository.GetAllAsync();

            return new
            {
                totalUsers = users.Count,
                activeUsers = users.Count(u => u.IsActive),
                totalTrades = trades.Count,
                totalVolume = trades.Sum(t => t.Total)
            };
        }
    }
}
