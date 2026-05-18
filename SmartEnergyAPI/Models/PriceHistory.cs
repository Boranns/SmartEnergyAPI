namespace SmartEnergyAPI.Models
{
    public class PriceHistory
    {
        public int Id { get; set; }
        public int EnergyProductId { get; set; }
        public decimal Price { get; set; }
        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

        public EnergyProduct EnergyProduct { get; set; } = null!;

    }
}
