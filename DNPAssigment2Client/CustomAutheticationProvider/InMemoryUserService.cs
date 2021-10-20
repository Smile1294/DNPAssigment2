using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DNPAssigment1.Login
{
    public class InMemoryUserService : IUserService
    {

        public async Task<User> ValidateUser(string username, string password)
        {
            var client = new HttpClient();
            var user = new User()
            {
                Username = username,
                Password = password,
            };
            var userAsJson = JsonSerializer.Serialize(user);
            var content = new StringContent(
                userAsJson,
                Encoding.UTF8,
                "application/json"
            );
            var response = await client.PostAsync("https://localhost:5003/Users", content);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode} : {response.ReasonPhrase}");
            var userAsString = await response.Content.ReadAsStringAsync();
            var loggedInUser = JsonSerializer.Deserialize<User>(userAsString, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return loggedInUser;
        }
    }
}