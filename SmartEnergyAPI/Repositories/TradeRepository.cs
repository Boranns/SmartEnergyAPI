using Microsoft.EntityFrameworkCore;
using SmartEnergyAPI.Data;
using SmartEnergyAPI.Models;

namespace SmartEnergyAPI.Repositories
{
    public class TradeRepository : ITradeRepository
    {
        private readonly AppDbContext _context;

        public TradeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Trade>> GetByUserIdAsync(int userId)
        {
            return await _context.Trades
                .Include(t => t.EnergyProduct)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.ExecutedAt)
                .ToListAsync();
        }

        public async Task<List<Trade>> GetAllAsync()
        {
            return await _context.Trades
                .Include(t => t.User)
                .Include(t => t.EnergyProduct)
                .OrderByDescending(t => t.ExecutedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Trade trade)
        {
            await _context.Trades.AddAsync(trade);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}