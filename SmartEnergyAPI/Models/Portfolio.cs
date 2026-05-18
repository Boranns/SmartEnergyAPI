namespace SmartEnergyAPI.Models
{
    public class Portfolio
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EnergyProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal AverageBuyPrice { get; set; }

        public User User { get; set; } = null!;
        public EnergyProduct EnergyProduct { get; set; } = null!;
    }
}
