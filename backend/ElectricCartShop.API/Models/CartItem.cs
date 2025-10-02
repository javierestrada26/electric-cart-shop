using System.Text.Json.Serialization;

namespace ElectricCartShop.API.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string? Size { get; set; }
        public string SessionId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property - not persisted in JSON, populated at runtime
        [JsonIgnore]
        public Product? Product { get; set; }
    }
}



