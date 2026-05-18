namespace SmartEnergyAPI.Models
{
    public class Trade
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EnergyProductId { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total {  get; set; }
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;
        public EnergyProduct EnergyProduct { get; set; } = null!;

    }
}
