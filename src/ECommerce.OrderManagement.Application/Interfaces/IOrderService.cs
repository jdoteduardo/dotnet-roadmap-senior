using ECommerce.OrderManagement.Application.DTOs;

namespace ECommerce.OrderManagement.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrderAsync(CreateOrderDTO createOrder);
        Task<OrderDTO?> GetOrderById(int id);
    }
}