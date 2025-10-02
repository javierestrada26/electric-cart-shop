namespace ElectricCartShop.API.DTOs
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public ProductDto Product { get; set; } = null!;
        public int Quantity { get; set; }
        public string? Size { get; set; }
    }

    public class AddToCartDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string? Size { get; set; }
        public string SessionId { get; set; } = string.Empty;
    }

    public class UpdateCartItemDto
    {
        public int Quantity { get; set; }
    }

    public class CartDto
    {
        public List<CartItemDto> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
    }
}


