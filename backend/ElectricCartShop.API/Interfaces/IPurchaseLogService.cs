using ElectricCartShop.API.Models;

namespace ElectricCartShop.API.Interfaces
{
    public interface IPurchaseLogService
    {
        Task<IEnumerable<PurchaseLog>> GetAllAsync();
        Task<PurchaseLog?> GetByIdAsync(int id);
        Task<IEnumerable<PurchaseLog>> GetByOrderNumberAsync(string orderNumber);
        Task<PurchaseLog> LogPurchaseAsync(Order order, string logMessage, string? additionalData = null);
    }
}


