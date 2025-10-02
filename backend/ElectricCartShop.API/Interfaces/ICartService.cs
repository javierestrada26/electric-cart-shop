using ElectricCartShop.API.DTOs;

namespace ElectricCartShop.API.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(string sessionId);
        Task<CartItemDto> AddToCartAsync(AddToCartDto addToCartDto);
        Task<CartItemDto> UpdateCartItemAsync(int id, UpdateCartItemDto updateCartItemDto);
        Task<bool> RemoveFromCartAsync(int id);
        Task<bool> ClearCartAsync(string sessionId);
        Task<CheckoutResponseDto> CheckoutAsync(CheckoutDto checkoutDto);
    }
}


