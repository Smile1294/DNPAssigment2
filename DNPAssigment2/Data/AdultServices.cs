using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DNPAssigment1.Models;
using Models;

namespace DNPAssigment1.Persistance 
{
    public class AdultServices : IAdultServices
    {
        public List<Adult> Adults { get; private set; }

        private readonly string adultsFile = "adults.json";

        public AdultServices()
        {
            Adults = File.Exists(adultsFile) ? ReadData<Adult>(adultsFile) : new List<Adult>();
        }
        

        public async Task<Adult> getAdult(int id)
        {
            return Adults.FirstOrDefault(t => t.Id == id);
        }

        public async Task UpdateAdults(Adult adultd)
        {
            Adult adult = Adults.First(t => t.Id == adultd.Id);
            Adults.Remove(adult);
            Adults.Add(adultd);
            SaveChanges();  
        }

        public async Task<Adult> AddAdult(Adult adult)
        {
            int max = Adults.Max(adult => adult.Id);
            adult.Id = (++max);
            Adults.Add(adult);
            SaveChanges();
            return adult;
        }

       
        public async Task<List<Adult>> LoadAdultsAsync()
        {
            List<Adult> tmp = new List<Adult>(Adults);
            return tmp.ToList();
        }

        public async Task RemoveAdult(int ID)
        {
            Adult adult = Adults.First(t => t.Id == ID);
            Adults.Remove(adult);
            List<Adult> tmp= new List<Adult>(Adults);
            await SaveChanges();
        }

        public List<T> ReadData<T>(string s)
        {
            using (var jsonReader = File.OpenText(s))
            {
                return JsonSerializer.Deserialize<List<T>>(jsonReader.ReadToEnd());
            }
        }


        public async Task SaveChanges()
        {
            // storing persons
            string jsonAdults = JsonSerializer.Serialize(Adults, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            using (StreamWriter outputFile = new StreamWriter(adultsFile, false))
            {
                outputFile.Write(jsonAdults);
            }
        }
    }
}