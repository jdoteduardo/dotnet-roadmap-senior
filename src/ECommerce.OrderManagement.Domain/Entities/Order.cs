using ECommerce.OrderManagement.Domain.ValueObjects;
using ECommerce.OrderManagement.Domain.Events;

namespace ECommerce.OrderManagement.Domain.Entities
{
    public class Order
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public Money SubTotal { get; set; }
        public int? CouponId { get; set; }
        public int? AddressId { get; set; }
        public int CustomerId { get; set; }
        public Coupon? Coupon { get; set; }
        public Address? Address { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public void MarkAsCreated()
        {
            Status = "Created";
            AddDomainEvent(new OrderCreatedEvent(Id, CustomerId, SubTotal.Value));
        }

        public void MarkAsPaid()
        {
            Status = "Paid";
            AddDomainEvent(new OrderPaidEvent(Id, CustomerId, SubTotal.Value));
        }
    }
}
