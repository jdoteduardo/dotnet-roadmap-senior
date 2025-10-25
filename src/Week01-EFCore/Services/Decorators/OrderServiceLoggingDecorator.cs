using Week01_EFCore.DTOs;
using Week01_EFCore.Entities;
using Week01_EFCore.Interfaces;

namespace Week01_EFCore.Services.Decorators
{
    public class OrderServiceLoggingDecorator : IOrderService
    {
        private readonly IOrderService _orderService;
        private readonly ILoggerService _logger;

        public OrderServiceLoggingDecorator(IOrderService orderService, ILoggerService logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderDTO createOrder)
        {
            await _logger.LogInformationAsync($"Iniciando criação de pedido com {createOrder.Items.Count} items");
            
            try
            {
                var order = await _orderService.CreateOrderAsync(createOrder);
                await _logger.LogInformationAsync($"Pedido {order.Id} criado com sucesso. Valor total: {order.SubTotal:C}");
                return order;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Erro ao criar pedido", ex);
                throw;
            }
        }
    }
}