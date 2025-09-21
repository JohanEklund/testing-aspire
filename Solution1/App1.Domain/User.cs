namespace App1.Domain
{
    public class User
    {
        public required Guid Key { get; set; }
        public required int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
