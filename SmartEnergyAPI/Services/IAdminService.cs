using SmartEnergyAPI.DTOs;
using SmartEnergyAPI.Models;

namespace SmartEnergyAPI.Services
{
    public interface IAdminService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<bool> ToggleUserStatusAsync(int id);
        Task<object> GetDashboardStatsAsync();
    }
}