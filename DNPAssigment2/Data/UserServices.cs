using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using DNPAssigment1.Login;

namespace DNPAssigment2.Data
{

    public class UserService : IUserService

    {
        private string usersFile = "users.json";
        public IList<User> Users { get; private set; }

        public UserService()
        {
            Users = File.Exists(usersFile) ? ReadData<User>(usersFile) : new List<User>();
            Seed();
            SaveChanges();
            
        }

        private void Seed()
        {
            Users = new[]
            {
                new User
                {
                    Password = "admin",
                    Username = "admin",
                    SecurityLvl = 10,
                    Age = 21
                }
            }.ToList();
        }
        public IList<T> ReadData<T>(string s)
        {
            using (var jsonReader = File.OpenText(s))
            {
                return JsonSerializer.Deserialize<List<T>>(jsonReader.ReadToEnd());
            }
        }
        public void SaveChanges()
        {
            // storing persons
            string users = JsonSerializer.Serialize(Users, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            using (StreamWriter outputFile = new StreamWriter(usersFile, false))
            {
                outputFile.Write(users);
            }
        }

        public async Task<User> ValidateUser(string userName, string passWord)
        {
            User user = Users.FirstOrDefault(u => u.Username.Equals(userName) && u.Password.Equals(passWord));
            if (user != null)
            {
                return user;
            } 
            throw new Exception("User not found");
        }
    }

}