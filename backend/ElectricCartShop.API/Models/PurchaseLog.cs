namespace ElectricCartShop.API.Models
{
    public class PurchaseLog
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
        public string LogMessage { get; set; } = string.Empty;
        public string? AdditionalData { get; set; }
    }
}


