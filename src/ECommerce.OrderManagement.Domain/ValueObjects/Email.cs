namespace ECommerce.OrderManagement.Domain.ValueObjects
{
    public class Email
    {
        public string Value { get; set; }

        public Email() { }

        public Email(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}