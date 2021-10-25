using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DNPAssigment1.Models;
using Models;

namespace DNPAssigment1.Persistance 
{
    public class AdultServices : IAdultServices
    {        

        public IList<Adult> Adults { get; private set; }
        private readonly HttpClient client;
        
        public AdultServices()
        {
            Adults = new List<Adult>();
            client = new HttpClient();
        }

        
        public async Task<Adult> GetAdultAsync(int id)
        {
            var responseMessage = await client.GetAsync($"https://localhost:5003/Adults/{id}");
            var content = await responseMessage.Content.ReadAsStringAsync();
            Adult adult = JsonSerializer.Deserialize<Adult>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return adult;

        }
        public async Task<IList<Adult>> GetAdultsAsync()
        {
            var responseMessage = await client.GetAsync("https://localhost:5003/Adults");
            var content = await responseMessage.Content.ReadAsStringAsync();
            Adults = JsonSerializer.Deserialize<List<Adult>>(content);
            return Adults;
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
            Random rnd = new Random();
            adult.Id = rnd.Next();
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
        public async Task RemoveAdultAsync(int ID)
        {
            var responseMessage = await client.DeleteAsync($"https://localhost:5003/Adults/{ID}");
            if(!responseMessage.IsSuccessStatusCode)
                throw new Exception($"Error: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");
        }

    }
}