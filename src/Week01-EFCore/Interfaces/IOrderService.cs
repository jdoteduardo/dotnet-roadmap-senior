using Week01_EFCore.DTOs;

namespace Week01_EFCore.Interfaces
{
    public interface IOrderService<Order>
    {
        Task<Order> CreateOrderAsync(CreateOrderDTO createOrder);
    }
}
