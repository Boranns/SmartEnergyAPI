using System.Diagnostics;

namespace SmartEnergyAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Trader";
        public decimal Balance { get; set; } = 10000;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        
        
        public ICollection<Trade> Trades { get; set; } = new List<Trade>();
        public ICollection<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
