using MediatR;
using ECommerce.OrderManagement.Domain.Events;

namespace ECommerce.OrderManagement.Application.Events
{
    public class OrderPaidEventHandler : INotificationHandler<OrderPaidEvent>
    {
        public Task Handle(OrderPaidEvent notification, CancellationToken cancellationToken)
        {   
            Console.WriteLine($"Payment received for Order {notification.OrderId} - Amount: {notification.AmountPaid:C}");
            
            return Task.CompletedTask;
        }
    }
}