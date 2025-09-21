namespace App2.Domain
{
    public class Order
    {
        public required Guid Key { get; set; }
        public required int UserId { get; set; }
        public required string OrderNumber { get; set; }
        public required DateTimeOffset OrderDate { get; set; }
    }
}
