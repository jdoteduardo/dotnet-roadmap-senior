namespace ECommerce.OrderManagement.Domain.ValueObjects
{
    public class Money
    {
        public decimal Value { get; set; }
        public string Currency { get; set; } = "BRL";

        public Money() { }

        public Money(decimal value)
        {
            Value = value;
        }

        public Money(decimal value, string currency)
        {
            Value = value;
            Currency = currency;
        }

        public override string ToString()
        {
            return $"{Value:C}";
        }
    }
}