using System.Collections.Generic;
using System.Threading.Tasks;
using DNPAssigment1.Models;
using Models;

namespace DNPAssigment1.Persistance
{
    public interface IAdultServices
    {
        public Task<Adult> getAdult(int id);
        public Task UpdateAdults(Adult adult);
        public Task<Adult> AddAdult(Adult adult);
        public Task<List<Adult>> LoadAdultsAsync();

        public Task<List<Adult>> RemoveAdult(int ID);



    }
}