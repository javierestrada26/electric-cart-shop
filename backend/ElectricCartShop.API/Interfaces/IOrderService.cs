using ElectricCartShop.API.DTOs;

namespace ElectricCartShop.API.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<OrderDto?> GetByIdAsync(int id);
        Task<OrderDto?> GetByOrderNumberAsync(string orderNumber);
        Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto createOrderDto);
    }
}


