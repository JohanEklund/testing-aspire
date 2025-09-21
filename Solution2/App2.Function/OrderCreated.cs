namespace App2.Function
{
    internal class OrderCreated
    {
        public required int UserId { get; set; }
        public required string OrderNumber { get; set; }
        public required DateTimeOffset OrderDate { get; set; }
    }
}
