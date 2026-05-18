using Microsoft.EntityFrameworkCore;
using SmartEnergyAPI.Data;
using SmartEnergyAPI.Models;

namespace SmartEnergyAPI.Repositories
{
    public class EnergyProductRepository : IEnergyProductRepository
    {
        private readonly AppDbContext _context;

        public EnergyProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<EnergyProduct>> GetAllAsync()
        {
            return await _context.EnergyProducts.ToListAsync();
        }

        public async Task<EnergyProduct?> GetByIdAsync(int id)
        {
            return await _context.EnergyProducts.FindAsync(id);
        }

        public async Task<EnergyProduct?> GetBySymbolAsync(string symbol)
        {
            return await _context.EnergyProducts
                .FirstOrDefaultAsync(e => e.Symbol == symbol);
        }

        public async Task<bool> AnyAsync()
        {
            return await _context.EnergyProducts.AnyAsync();
        }

        public async Task AddRangeAsync(List<EnergyProduct> products)
        {
            await _context.EnergyProducts.AddRangeAsync(products);
        }

        public async Task UpdateAsync(EnergyProduct product)
        {
            _context.EnergyProducts.Update(product);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
