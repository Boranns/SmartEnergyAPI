namespace SmartEnergyAPI.DTOs
{
    public class TradeDto
    {
        public int EnergyProductId {  get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
    }
}
