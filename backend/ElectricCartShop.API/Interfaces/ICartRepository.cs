using ElectricCartShop.API.Models;

namespace ElectricCartShop.API.Interfaces
{
    public interface ICartRepository
    {
        Task<IEnumerable<CartItem>> GetBySessionIdAsync(string sessionId);
        Task<CartItem?> GetByIdAsync(int id);
        Task<CartItem> CreateAsync(CartItem cartItem);
        Task<CartItem> UpdateAsync(CartItem cartItem);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteBySessionIdAsync(string sessionId);
        Task<bool> ExistsAsync(int id);
    }
}


