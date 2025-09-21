namespace PublisherConsole
{
    internal class UserOrderCreated
    {
        public required int UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string OrderNumber { get; set; }
        public required DateTimeOffset OrderDate { get; set; }
    }
}
