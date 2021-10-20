using System.Text.Json.Serialization;
using Models;

namespace DNPAssigment1.Models {
    public class Adult : Person {
    
        public Job JobTitle { get; set; }
    }   
}