using Week01_EFCore.DTOs;
using Week01_EFCore.Entities;

namespace Week01_EFCore.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(CreateOrderDTO createOrder);
    }
}
