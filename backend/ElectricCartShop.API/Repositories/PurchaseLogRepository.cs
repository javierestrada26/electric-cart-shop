using ElectricCartShop.API.Interfaces;
using ElectricCartShop.API.Models;
using ElectricCartShop.API.Services;

namespace ElectricCartShop.API.Repositories
{
    public class PurchaseLogRepository : IPurchaseLogRepository
    {
        private readonly IJsonDatabaseService _databaseService;

        public PurchaseLogRepository(IJsonDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<PurchaseLog>> GetAllAsync()
        {
            var data = await _databaseService.LoadDataAsync();
            return data.PurchaseLogs;
        }

        public async Task<PurchaseLog?> GetByIdAsync(int id)
        {
            var data = await _databaseService.LoadDataAsync();
            return data.PurchaseLogs.FirstOrDefault(p => p.Id == id);
        }

        public async Task<IEnumerable<PurchaseLog>> GetByOrderNumberAsync(string orderNumber)
        {
            var data = await _databaseService.LoadDataAsync();
            return data.PurchaseLogs.Where(p => p.OrderNumber == orderNumber);
        }

        public async Task<PurchaseLog> CreateAsync(PurchaseLog purchaseLog)
        {
            var data = await _databaseService.LoadDataAsync();
            
            purchaseLog.Id = data.Counters.PurchaseLogId;
            purchaseLog.PurchaseDate = DateTime.UtcNow;
            
            data.PurchaseLogs.Add(purchaseLog);
            data.Counters.PurchaseLogId++;
            
            await _databaseService.SaveDataAsync(data);
            return purchaseLog;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var data = await _databaseService.LoadDataAsync();
            return data.PurchaseLogs.Any(p => p.Id == id);
        }
    }
}


