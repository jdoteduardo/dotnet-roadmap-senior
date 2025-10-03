namespace Week01_EFCore.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
