using ElectricCartShop.API.Models;

namespace ElectricCartShop.API.Interfaces
{
    public interface IPurchaseLogRepository
    {
        Task<IEnumerable<PurchaseLog>> GetAllAsync();
        Task<PurchaseLog?> GetByIdAsync(int id);
        Task<IEnumerable<PurchaseLog>> GetByOrderNumberAsync(string orderNumber);
        Task<PurchaseLog> CreateAsync(PurchaseLog purchaseLog);
        Task<bool> ExistsAsync(int id);
    }
}


