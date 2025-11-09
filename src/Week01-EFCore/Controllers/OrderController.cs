using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Week01_EFCore.DTOs;
using Week01_EFCore.Interfaces;
using Week01_EFCore.Services;

namespace Week01_EFCore.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO?>> GetById(int id)
        {
            var order = await _orderService.GetOrderById(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> CreateOrder(CreateOrderDTO createOrder)
        {
            var createdOrder = await _orderService.CreateOrderAsync(createOrder);
            return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
        }
    }
}
