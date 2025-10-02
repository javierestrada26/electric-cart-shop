using ElectricCartShop.API.DTOs;
using ElectricCartShop.API.Interfaces;
using ElectricCartShop.API.Models;

namespace ElectricCartShop.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPurchaseLogService _purchaseLogService;

        public OrderService(
            IOrderRepository orderRepository, 
            IProductRepository productRepository,
            IPurchaseLogService purchaseLogService)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _purchaseLogService = purchaseLogService;
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(MapToDto);
        }

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return order != null ? MapToDto(order) : null;
        }

        public async Task<OrderDto?> GetByOrderNumberAsync(string orderNumber)
        {
            var order = await _orderRepository.GetByOrderNumberAsync(orderNumber);
            return order != null ? MapToDto(order) : null;
        }

        public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var order = new Order
            {
                CustomerEmail = createOrderDto.CustomerEmail,
                CustomerName = createOrderDto.CustomerName,
                Status = "Pending",
                OrderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            foreach (var itemDto in createOrderDto.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(itemDto.Product.Id);
                if (product == null)
                    throw new ArgumentException($"Product with ID {itemDto.Product.Id} not found.");

                var orderItem = new OrderItem
                {
                    ProductId = itemDto.Product.Id,
                    Quantity = itemDto.Quantity,
                    Size = itemDto.Size,
                    UnitPrice = product.Price,
                    TotalPrice = product.Price * itemDto.Quantity
                };

                order.OrderItems.Add(orderItem);
                totalAmount += orderItem.TotalPrice;
            }

            order.TotalAmount = totalAmount;
            var createdOrder = await _orderRepository.CreateAsync(order);

            // Log the purchase
            var logMessage = $"Purchase completed successfully for order {createdOrder.OrderNumber}";
            await _purchaseLogService.LogPurchaseAsync(createdOrder, logMessage, 
                $"Customer: {createdOrder.CustomerName}, Email: {createdOrder.CustomerEmail}");

            return new OrderResponseDto
            {
                OrderNumber = createdOrder.OrderNumber,
                Status = createdOrder.Status,
                TotalAmount = createdOrder.TotalAmount,
                OrderDate = createdOrder.OrderDate,
                Message = "Order created successfully"
            };
        }

        private static OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CustomerEmail = order.CustomerEmail,
                CustomerName = order.CustomerName,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderDate = order.OrderDate,
                OrderItems = order.OrderItems.Select(MapOrderItemToDto).ToList()
            };
        }

        private static OrderItemDto MapOrderItemToDto(OrderItem orderItem)
        {
            return new OrderItemDto
            {
                Id = orderItem.Id,
                Product = new ProductDto
                {
                    Id = orderItem.Product.Id,
                    Name = orderItem.Product.Name,
                    Price = orderItem.Product.Price,
                    Image = orderItem.Product.Image,
                    Description = orderItem.Product.Description,
                    Category = orderItem.Product.Category,
                    Sizes = orderItem.Product.Sizes
                },
                Quantity = orderItem.Quantity,
                Size = orderItem.Size,
                UnitPrice = orderItem.UnitPrice,
                TotalPrice = orderItem.TotalPrice
            };
        }
    }
}


