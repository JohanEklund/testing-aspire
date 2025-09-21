using System.Net.Http.Json;

namespace App2.Infrastructure
{
    public class App1Client(HttpClient httpClient) : IApp1Client
    {
        public async Task<IEnumerable<App1User>> GetUsersAsync()
        {
            var response = await httpClient.GetAsync("user");
            response.EnsureSuccessStatusCode();
            var users = await response.Content.ReadFromJsonAsync<IEnumerable<App1User>>();
            return users ?? [];
        }
    }

    public class App1User
    {
        public required int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
