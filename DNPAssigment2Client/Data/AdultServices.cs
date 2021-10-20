using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DNPAssigment1.Models;
using Models;

namespace DNPAssigment1.Persistance 
{
    public class AdultServices : IAdultServices
    {        
        HttpClientHandler clientHandler = new HttpClientHandler();

        public IList<Adult> Adults { get; private set; }
        private readonly HttpClient client;
        
        public AdultServices()
        {
            Adults = new List<Adult>();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(clientHandler);
        }

        
        public async Task<Adult> GetAdultAsync(int id)
        {
            var responseMessage = await client.GetAsync($"https://localhost:5003/Adults/{id}");
            var content = await responseMessage.Content.ReadAsStringAsync();
            Adult adult = JsonSerializer.Deserialize<Adult>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            Console.WriteLine(adult);
            return adult;

        }

        public async Task UpdateAdultsAsync(Adult adult)
        {
            var adultAsJson = JsonSerializer.Serialize(adult);
            var content = new StringContent(
                adultAsJson,
                Encoding.UTF8,
                "application/json"
            );
            var responseMessage = await client.PatchAsync("https://localhost:5003/Adults",content);
            if(!responseMessage.IsSuccessStatusCode)
                throw new Exception($"Error: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");
        }

        public async Task AddAdultAsync(Adult adult)
        {
            int max = Adults.Max(adult => adult.Id);
            adult.Id = (++max);
            var adultAsJson = JsonSerializer.Serialize(adult);
            var content = new StringContent(
                adultAsJson,
                Encoding.UTF8,
                "application/json"
            );
            var responseMessage = await client.PutAsync("https://localhost:5003/Adults",content);
            if(!responseMessage.IsSuccessStatusCode)
                throw new Exception($"Error: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");
        }

       
        public async Task<IList<Adult>> GetAdultsAsync()
        {
            var responseMessage = await client.GetAsync("https://localhost:5003/Adults");
            var content = await responseMessage.Content.ReadAsStringAsync();
            Adults = JsonSerializer.Deserialize<IList<Adult>>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return Adults;
        }

        public async Task RemoveAdultAsync(int ID)
        {
            var responseMessage = await client.DeleteAsync($"https://localhost:5003/Adults/{ID}");
            if(!responseMessage.IsSuccessStatusCode)
                throw new Exception($"Error: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");
            
        }

    }
}