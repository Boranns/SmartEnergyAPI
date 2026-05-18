using System.Data;
using System.Diagnostics;

namespace SmartEnergyAPI.Models
{
    public class EnergyProduct
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public decimal CurrentPrice { get; set; }
        public decimal PreviousPrice { get; set; }
        public string Unit { get; set; } = "MWh";
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        public ICollection<Trade> Trades { get; set; } = new List<Trade>();
        public ICollection<PriceHistory> PriceHistories { get; set; } = new List<PriceHistory>();

    }
}
