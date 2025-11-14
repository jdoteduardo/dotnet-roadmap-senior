using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Application.Features.Orders.Commands.CreateOrder;
using ECommerce.OrderManagement.Application.Features.Orders.Queries.GetOrderById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.OrderManagement.API.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO?>> GetById(int id)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery { Id = id });

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> CreateOrder(CreateOrderCommand command)
        {
            var createdOrder = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
        }
    }
}
