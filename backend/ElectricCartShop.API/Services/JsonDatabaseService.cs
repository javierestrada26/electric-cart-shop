using ElectricCartShop.API.Models;
using System.Text.Json;

namespace ElectricCartShop.API.Services
{
    public class DatabaseData
    {
        public List<Product> Products { get; set; } = new();
        public List<CartItem> CartItems { get; set; } = new();
        public List<Order> Orders { get; set; } = new();
        public List<PurchaseLog> PurchaseLogs { get; set; } = new();
        public DatabaseCounters Counters { get; set; } = new();
    }

    public class DatabaseCounters
    {
        public int ProductId { get; set; } = 1;
        public int CartItemId { get; set; } = 1;
        public int OrderId { get; set; } = 1;
        public int PurchaseLogId { get; set; } = 1;
    }

    public interface IJsonDatabaseService
    {
        Task<DatabaseData> LoadDataAsync();
        Task SaveDataAsync(DatabaseData data);
        Task<int> GetNextIdAsync(string entityType);
        Task UpdateCounterAsync(string entityType, int newValue);
    }

    public class JsonDatabaseService : IJsonDatabaseService
    {
        private readonly string _dataFilePath;
        private readonly SemaphoreSlim _semaphore;
        private readonly ILogger<JsonDatabaseService> _logger;

        public JsonDatabaseService(IWebHostEnvironment environment, ILogger<JsonDatabaseService> logger)
        {
            _dataFilePath = Path.Combine(environment.ContentRootPath, "Data", "database.json");
            _semaphore = new SemaphoreSlim(1, 1);
            _logger = logger;

            // Ensure directory exists
            var directory = Path.GetDirectoryName(_dataFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }
        }

        public async Task<DatabaseData> LoadDataAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                if (!File.Exists(_dataFilePath))
                {
                    _logger.LogWarning("Database file not found, creating new one at: {FilePath}", _dataFilePath);
                    var newData = new DatabaseData();
                    await SaveDataInternalAsync(newData);
                    return newData;
                }

                var jsonString = await File.ReadAllTextAsync(_dataFilePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                };

                var data = JsonSerializer.Deserialize<DatabaseData>(jsonString, options);
                return data ?? new DatabaseData();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading database from file: {FilePath}", _dataFilePath);
                return new DatabaseData();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task SaveDataAsync(DatabaseData data)
        {
            await _semaphore.WaitAsync();
            try
            {
                await SaveDataInternalAsync(data);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task SaveDataInternalAsync(DatabaseData data)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                };

                var jsonString = JsonSerializer.Serialize(data, options);
                await File.WriteAllTextAsync(_dataFilePath, jsonString);
                _logger.LogInformation("Database saved successfully to: {FilePath}", _dataFilePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving database to file: {FilePath}", _dataFilePath);
                throw;
            }
        }

        public async Task<int> GetNextIdAsync(string entityType)
        {
            var data = await LoadDataAsync();
            return entityType.ToLower() switch
            {
                "product" => data.Counters.ProductId,
                "cartitem" => data.Counters.CartItemId,
                "order" => data.Counters.OrderId,
                "purchaselog" => data.Counters.PurchaseLogId,
                _ => throw new ArgumentException($"Unknown entity type: {entityType}")
            };
        }

        public async Task UpdateCounterAsync(string entityType, int newValue)
        {
            var data = await LoadDataAsync();
            switch (entityType.ToLower())
            {
                case "product":
                    data.Counters.ProductId = newValue;
                    break;
                case "cartitem":
                    data.Counters.CartItemId = newValue;
                    break;
                case "order":
                    data.Counters.OrderId = newValue;
                    break;
                case "purchaselog":
                    data.Counters.PurchaseLogId = newValue;
                    break;
                default:
                    throw new ArgumentException($"Unknown entity type: {entityType}");
            }
            await SaveDataAsync(data);
        }
    }
}
