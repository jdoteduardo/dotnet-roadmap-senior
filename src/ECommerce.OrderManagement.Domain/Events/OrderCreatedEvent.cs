namespace ECommerce.OrderManagement.Domain.Events
{
    public class OrderCreatedEvent : IDomainEvent
    {
        public int OrderId { get; private set; }
        public int CustomerId { get; private set; }
        public decimal TotalAmount { get; private set; }
        public DateTime OccurredOn { get; private set; }

        public OrderCreatedEvent(int orderId, int customerId, decimal totalAmount)
        {
            OrderId = orderId;
            CustomerId = customerId;
            TotalAmount = totalAmount;
            OccurredOn = DateTime.UtcNow;
        }
    }
}