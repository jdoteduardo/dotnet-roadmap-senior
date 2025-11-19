using MediatR;
using ECommerce.OrderManagement.Domain.Events;

namespace ECommerce.OrderManagement.Application.Events
{
    public class OrderCreatedEventHandler : INotificationHandler<OrderCreatedEvent>
    {
        public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {   
            Console.WriteLine($"Order {notification.OrderId} created for customer {notification.CustomerId}");
            
            return Task.CompletedTask;
        }
    }
}