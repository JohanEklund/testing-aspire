
namespace App2.Infrastructure
{
    public interface IApp1Client
    {
        Task<IEnumerable<App1User>> GetUsersAsync();
    }
}