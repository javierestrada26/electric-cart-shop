using ElectricCartShop.API.Interfaces;
using ElectricCartShop.API.Models;

namespace ElectricCartShop.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders;
        private int _nextId = 1;

        public OrderRepository()
        {
            _orders = new List<Order>();
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await Task.FromResult(_orders);
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await Task.FromResult(_orders.FirstOrDefault(o => o.Id == id));
        }

        public async Task<Order?> GetByOrderNumberAsync(string orderNumber)
        {
            return await Task.FromResult(_orders.FirstOrDefault(o => o.OrderNumber == orderNumber));
        }

        public async Task<Order> CreateAsync(Order order)
        {
            order.Id = _nextId++;
            order.OrderNumber = GenerateOrderNumber();
            order.OrderDate = DateTime.UtcNow;
            _orders.Add(order);
            return await Task.FromResult(order);
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            var existingOrder = _orders.FirstOrDefault(o => o.Id == order.Id);
            if (existingOrder != null)
            {
                existingOrder.CustomerEmail = order.CustomerEmail;
                existingOrder.CustomerName = order.CustomerName;
                existingOrder.TotalAmount = order.TotalAmount;
                existingOrder.Status = order.Status;
            }
            return await Task.FromResult(existingOrder!);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await Task.FromResult(_orders.Any(o => o.Id == id));
        }

        private string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{_nextId:D4}";
        }
    }
}


