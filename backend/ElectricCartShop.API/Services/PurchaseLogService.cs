using ElectricCartShop.API.Interfaces;
using ElectricCartShop.API.Models;

namespace ElectricCartShop.API.Services
{
    public class PurchaseLogService : IPurchaseLogService
    {
        private readonly IPurchaseLogRepository _purchaseLogRepository;

        public PurchaseLogService(IPurchaseLogRepository purchaseLogRepository)
        {
            _purchaseLogRepository = purchaseLogRepository;
        }

        public async Task<IEnumerable<PurchaseLog>> GetAllAsync()
        {
            return await _purchaseLogRepository.GetAllAsync();
        }

        public async Task<PurchaseLog?> GetByIdAsync(int id)
        {
            return await _purchaseLogRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<PurchaseLog>> GetByOrderNumberAsync(string orderNumber)
        {
            return await _purchaseLogRepository.GetByOrderNumberAsync(orderNumber);
        }

        public async Task<PurchaseLog> LogPurchaseAsync(Order order, string logMessage, string? additionalData = null)
        {
            var purchaseLog = new PurchaseLog
            {
                OrderNumber = order.OrderNumber,
                CustomerEmail = order.CustomerEmail,
                CustomerName = order.CustomerName,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                LogMessage = logMessage,
                AdditionalData = additionalData
            };

            return await _purchaseLogRepository.CreateAsync(purchaseLog);
        }
    }
}


