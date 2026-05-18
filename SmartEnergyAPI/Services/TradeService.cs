using SmartEnergyAPI.DTOs;
using SmartEnergyAPI.Models;
using SmartEnergyAPI.Repositories;

namespace SmartEnergyAPI.Services
{
    public class TradeService : ITradeService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEnergyProductRepository _energyProductRepository;
        private readonly ITradeRepository _tradeRepository;
        private readonly IPortfolioRepository _portfolioRepository;

        public TradeService(
            IUserRepository userRepository,
            IEnergyProductRepository energyProductRepository,
            ITradeRepository tradeRepository,
            IPortfolioRepository portfolioRepository)
        {
            _userRepository = userRepository;
            _energyProductRepository = energyProductRepository;
            _tradeRepository = tradeRepository;
            _portfolioRepository = portfolioRepository;
        }

        public async Task<Trade> ExecuteTradeAsync(int userId, TradeDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new Exception("Brugeren blev ikke fundet.");

            var product = await _energyProductRepository.GetByIdAsync(dto.EnergyProductId)
                ?? throw new Exception("Energiproduktet blev ikke fundet.");

            var total = dto.Quantity * product.CurrentPrice;

            if (dto.Type == "Buy")
            {
                if (user.Balance < total)
                    throw new Exception("Utilstrækkelig saldo.");

                user.Balance -= total;

                var portfolio = await _portfolioRepository.GetByUserAndProductAsync(userId, dto.EnergyProductId);

                if (portfolio == null)
                {
                    await _portfolioRepository.AddAsync(new Portfolio
                    {
                        UserId = userId,
                        EnergyProductId = dto.EnergyProductId,
                        Quantity = dto.Quantity,
                        AverageBuyPrice = product.CurrentPrice
                    });
                }
                else
                {
                    var totalQuantity = portfolio.Quantity + dto.Quantity;
                    portfolio.AverageBuyPrice = ((portfolio.Quantity * portfolio.AverageBuyPrice) + (dto.Quantity * product.CurrentPrice)) / totalQuantity;
                    portfolio.Quantity = totalQuantity;
                    await _portfolioRepository.UpdateAsync(portfolio);
                }
            }
            else if (dto.Type == "Sell")
            {
                var portfolio = await _portfolioRepository.GetByUserAndProductAsync(userId, dto.EnergyProductId)
                    ?? throw new Exception("Du har ikke dette energiprodukt i din portefølje.");

                if (portfolio.Quantity < dto.Quantity)
                    throw new Exception("Utilstrækkelig mængde i porteføljen.");

                portfolio.Quantity -= dto.Quantity;
                user.Balance += total;

                if (portfolio.Quantity == 0)
                    await _portfolioRepository.RemoveAsync(portfolio);
                else
                    await _portfolioRepository.UpdateAsync(portfolio);
            }
            else
            {
                throw new Exception("Ugyldig handelstype. Brug 'Buy' eller 'Sell'.");
            }

            var trade = new Trade
            {
                UserId = userId,
                EnergyProductId = dto.EnergyProductId,
                Type = dto.Type,
                Quantity = dto.Quantity,
                Price = product.CurrentPrice,
                Total = total,
                ExecutedAt = DateTime.UtcNow
            };

            await _tradeRepository.AddAsync(trade);
            await _userRepository.UpdateAsync(user);
            await _tradeRepository.SaveChangesAsync();

            return trade;
        }

        public async Task<List<Trade>> GetUserTradesAsync(int userId)
        {
            return await _tradeRepository.GetByUserIdAsync(userId);
        }

        public async Task<List<Trade>> GetAllTradesAsync()
        {
            return await _tradeRepository.GetAllAsync();
        }
    }
}
