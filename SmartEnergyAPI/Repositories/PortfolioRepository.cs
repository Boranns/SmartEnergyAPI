using Microsoft.EntityFrameworkCore;
using SmartEnergyAPI.Data;
using SmartEnergyAPI.Models;

namespace SmartEnergyAPI.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly AppDbContext _context;

        public PortfolioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Portfolio>> GetByUserIdAsync(int userId)
        {
            return await _context.Portfolios
                .Include(p => p.EnergyProduct)
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<Portfolio?> GetByUserAndProductAsync(int userId, int energyProductId)
        {
            return await _context.Portfolios
                .FirstOrDefaultAsync(p => p.UserId == userId && p.EnergyProductId == energyProductId);
        }

        public async Task AddAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
        }

        public async Task UpdateAsync(Portfolio portfolio)
        {
            _context.Portfolios.Update(portfolio);
        }

        public async Task RemoveAsync(Portfolio portfolio)
        {
            _context.Portfolios.Remove(portfolio);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
