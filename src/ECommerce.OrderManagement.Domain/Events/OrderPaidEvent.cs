namespace ECommerce.OrderManagement.Domain.Events
{
    public class OrderPaidEvent : IDomainEvent
    {
        public int OrderId { get; private set; }
        public int CustomerId { get; private set; }
        public decimal AmountPaid { get; private set; }
        public DateTime OccurredOn { get; private set; }

        public OrderPaidEvent(int orderId, int customerId, decimal amountPaid)
        {
            OrderId = orderId;
            CustomerId = customerId;
            AmountPaid = amountPaid;
            OccurredOn = DateTime.UtcNow;
        }
    }
}