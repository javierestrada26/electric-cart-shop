using ElectricCartShop.API.Interfaces;
using ElectricCartShop.API.Models;
using ElectricCartShop.API.Services;

namespace ElectricCartShop.API.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IJsonDatabaseService _databaseService;

        public CartRepository(IJsonDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<CartItem>> GetBySessionIdAsync(string sessionId)
        {
            var data = await _databaseService.LoadDataAsync();
            return data.CartItems.Where(c => c.SessionId == sessionId);
        }

        public async Task<CartItem?> GetByIdAsync(int id)
        {
            var data = await _databaseService.LoadDataAsync();
            return data.CartItems.FirstOrDefault(c => c.Id == id);
        }

        public async Task<CartItem> CreateAsync(CartItem cartItem)
        {
            var data = await _databaseService.LoadDataAsync();
            
            cartItem.Id = data.Counters.CartItemId;
            cartItem.CreatedAt = DateTime.UtcNow;
            cartItem.UpdatedAt = DateTime.UtcNow;
            
            data.CartItems.Add(cartItem);
            data.Counters.CartItemId++;
            
            await _databaseService.SaveDataAsync(data);
            return cartItem;
        }

        public async Task<CartItem> UpdateAsync(CartItem cartItem)
        {
            var data = await _databaseService.LoadDataAsync();
            var existingItem = data.CartItems.FirstOrDefault(c => c.Id == cartItem.Id);
            
            if (existingItem != null)
            {
                existingItem.Quantity = cartItem.Quantity;
                existingItem.Size = cartItem.Size;
                existingItem.UpdatedAt = DateTime.UtcNow;
                
                await _databaseService.SaveDataAsync(data);
            }
            
            return existingItem!;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var data = await _databaseService.LoadDataAsync();
            var cartItem = data.CartItems.FirstOrDefault(c => c.Id == id);
            
            if (cartItem != null)
            {
                data.CartItems.Remove(cartItem);
                await _databaseService.SaveDataAsync(data);
                return true;
            }
            
            return false;
        }

        public async Task<bool> DeleteBySessionIdAsync(string sessionId)
        {
            var data = await _databaseService.LoadDataAsync();
            var itemsToRemove = data.CartItems.Where(c => c.SessionId == sessionId).ToList();
            
            foreach (var item in itemsToRemove)
            {
                data.CartItems.Remove(item);
            }
            
            if (itemsToRemove.Any())
            {
                await _databaseService.SaveDataAsync(data);
                return true;
            }
            
            return false;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var data = await _databaseService.LoadDataAsync();
            return data.CartItems.Any(c => c.Id == id);
        }
    }
}


