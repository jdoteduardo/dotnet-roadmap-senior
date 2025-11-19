using MediatR;

namespace ECommerce.OrderManagement.Domain.Events
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }
}